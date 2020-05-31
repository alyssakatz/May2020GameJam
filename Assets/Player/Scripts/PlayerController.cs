using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoSingleton<PlayerController>
{
    public GameObject playedCardPrefab;
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rigidBody => GetComponent<Rigidbody>();

    [ReadOnly]
    [SerializeField]
    private Vector2 MovementVector;

    public float MinimumMovementThresholdl = 0.05f;

    public float MovementSpeed = 1.0f;
    public float JumpSpeed = 1.0f;

    public bool IsGrounded = true;
    public bool IsAnchored = false;

    public float ContinuousTargetingDelay = 0.5f;
    private Int3? _targetPosition;
    private float _holdTime = 0.0f;
    private int _samples = 0;
    private Vector2 _sumTargetingDirection = new Vector2(0, 0);
    public string debug;

    private void OnDrawGizmos()
    {
        if (!_targetPosition || !BlockSystemManager.Instance.AlignedSystem.IsInBlockSystem((Int3)_targetPosition)) return;
        Block block = BlockSystemManager.Instance.AlignedSystem.GetBlockByLocation((Int3)_targetPosition);
        if (block)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(block.transform.position, 0.5f);
            
        }
    }
    void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Move.started += ctx => MovementVector = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Move.performed += ctx => MovementVector = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Jump.started += OnJump;

        SubscribeToCardEvents(_playerInputActions.Player.PlayCard1, () => Playspace.Instance.LeftCard);
        SubscribeToCardEvents(_playerInputActions.Player.PlayCard2, () => Playspace.Instance.MiddleCard);
        SubscribeToCardEvents(_playerInputActions.Player.PlayCard3, () => Playspace.Instance.RightCard);

        _playerInputActions.Player.Escape.started += OnEscape;

        //this is the wrong place to put this... but I need to keep moving here.
        Playspace.Instance.Initialize(Enumerable.Range(0, Cards.CardList.Length).ToArray());
        Playspace.Instance.DrawCards(false);
    }

    /* Initially interpret input as a tap.
     * If the button is pressed for too long for it to be a tap, cancel the tap action and start the slowtap action.
     * If the button is released, perform the action
     */
    void SubscribeToCardEvents(InputAction input, Func<CardInfo?> cardLocation)
    {
        input.started +=
            context =>
            {
                CardInfo? card = cardLocation();
                if (card != null && context.interaction is SlowTapInteraction)
                {
                    if (card.Value.CanTarget)
                    {
                        Debug.Log("TARGET" + context.interaction);
                        OnTarget(context, (CardInfo)card);
                    }
                    else
                    {
                        Debug.Log("Forward target to play, since card is not targettable." + context.interaction);
                        OnPlayCard(context, (CardInfo)card);
                    }
                    
                }
            };

        input.performed +=
            context =>
            {
                CardInfo? card = cardLocation();
                if (card != null)
                {
                    Debug.Log("PLAY" + context.interaction);
                    OnPlayCard(context, (CardInfo) card);
                }
            };
    }

    void FixedUpdate()
    {
        debug = $"Cards: {Playspace.Instance.LeftCard?.Name} {Playspace.Instance.MiddleCard?.Name} {Playspace.Instance.RightCard?.Name}";
        if (IsAnchored)
        {
            _holdTime += Time.deltaTime;
            _samples += 1;
            _sumTargetingDirection += ApplyThresholding(_playerInputActions.Player.Move.ReadValue<Vector2>());
            
            if (_holdTime >= ContinuousTargetingDelay)
            {
                Vector2 averageTargetingDirection = ApplyThresholding(_sumTargetingDirection / _samples);

                //convert to correct int3 offset
                if(Math.Abs(averageTargetingDirection.x) > Math.Abs(averageTargetingDirection.y))
                {
                    averageTargetingDirection.y = 0;
                }else
                {
                    averageTargetingDirection.x = 0;
                }

                Int3 offset = new Int3(Math.Sign(averageTargetingDirection.x), 0, Math.Sign(averageTargetingDirection.y));

                _targetPosition += offset;

                // Reset
                _holdTime -= ContinuousTargetingDelay;
                _samples = 0;
                _sumTargetingDirection = new Vector2(0, 0);
            }
        }
        else
        {
            Vector2 movementVector = ApplyThresholding(_playerInputActions.Player.Move.ReadValue<Vector2>());
            Vector3 velocity = new Vector3(movementVector.x, 0, movementVector.y) * MovementSpeed;

            // Respect velocity from jumping
            velocity.y = _rigidBody.velocity.y;

            _rigidBody.velocity = velocity;
        }
    }

    // Negates controller drift
    Vector2 ApplyThresholding(Vector2 vector)
    {
        if (Math.Abs(vector.x) < MinimumMovementThresholdl) vector.x = 0;
        if (Math.Abs(vector.y) < MinimumMovementThresholdl) vector.y = 0;
        return vector;
    }

    void OnEnable()
    {
        _playerInputActions.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.Disable();
    }

    public void OnPlayCard(InputAction.CallbackContext context, CardInfo card)
    {
        Debug.Log("Playing card " + card);
        CardTargettingInfo info;

        if (card.BaseRange.HasValue)
        {
            if (!_targetPosition) _targetPosition = CalculateBaseTargetPosition((int)card.BaseRange);
            info = new CardTargettingInfo((Int3)_targetPosition);
        }else
        {
            info = new CardTargettingInfo(null);
        }

        CardLoading memento = Instantiate(playedCardPrefab).GetComponent<CardLoading>();
        memento.CardInfo = card;
        memento.CardTargettingInfo = info;
        memento.source = GetCurrentBlock();

        IsAnchored = false;
        _targetPosition = null;
        Debug.Log("Playing card " + card);
    }

    public void OnTarget(InputAction.CallbackContext context, CardInfo card)
    {
        IsAnchored = true;
        _holdTime = 0.0f;
        _samples = 0;
        _sumTargetingDirection = new Vector2();

        //set initial targetted block
        Int3 currentPosition = BlockSystemManager.Instance.AlignedSystem.GetBlockLocationByWorldPosition(transform.position).AlignedLocation;

        _targetPosition = CalculateBaseTargetPosition((int) card.BaseRange);

        Debug.Log("Targeting with card " + card + " starting at location " + _targetPosition);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            Vector3 impulse = new Vector3(0, 1, 0) * JumpSpeed;
            _rigidBody.AddForce(impulse, ForceMode.Impulse);
            IsGrounded = false;
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        Debug.Log("Escape");
    }

    private Block GetCurrentBlock()
    {
        return BlockSystemManager.Instance.AlignedSystem.GetBlockLocationByWorldPosition(transform.position);
    }
    private Int3 CalculateBaseTargetPosition(int range)
    {
        Int3 currentPosition = GetCurrentBlock().AlignedLocation;

        return BlockSystemManager.Instance.AlignedSystem.GetLocationAtRange(currentPosition, Int3.PlusX, range);
    }
}
