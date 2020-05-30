using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoSingleton<PlayerController>
{
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

        SubscribeToCardEvents(_playerInputActions.Player.PlayCard1, Playspace.Instance.LeftCard);
        SubscribeToCardEvents(_playerInputActions.Player.PlayCard2, Playspace.Instance.MiddleCard);
        SubscribeToCardEvents(_playerInputActions.Player.PlayCard3, Playspace.Instance.RightCard);

        _playerInputActions.Player.Escape.started += OnEscape;
    }

    /* Initially interpret input as a tap.
     * If the button is pressed for too long for it to be a tap, cancel the tap action and start the slowtap action.
     * If the button is released, perform the action
     */
    void SubscribeToCardEvents(InputAction input, CardInfo? card)
    {
        card = Cards.CardList[1]; //the playspace is broken, so I do this.
        input.started +=
            context =>
            {
                
                if (card != null && card.Value.CanTarget && context.interaction is SlowTapInteraction)
                {
                    Debug.Log("TARGET" + context.interaction);
                    OnTarget(context, (CardInfo) card);
                }
            };

        input.performed +=
            context =>
            {
                
                if (card != null)
                {
                    Debug.Log("PLAY" + context.interaction);
                    OnPlayCard(context, (CardInfo) card);
                }
            };
    }

    void FixedUpdate()
    {
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

        if (!_targetPosition && card.BaseRange.HasValue)
        {
            _targetPosition = CalculateBaseTargetPosition((int)card.BaseRange);
        }

        CardTargettingInfo info = new CardTargettingInfo((Int3) _targetPosition);

        //this isn't supposed to be how cards are played, there's supposed to be a delay, but ignore that for now.
        card.Execute(info);

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

    private Int3 CalculateBaseTargetPosition(int range)
    {
        Int3 currentPosition = BlockSystemManager.Instance.AlignedSystem.GetBlockLocationByWorldPosition(transform.position).AlignedLocation;

        return BlockSystemManager.Instance.AlignedSystem.GetLocationAtRange(currentPosition, Int3.PlusX, range);
    }
}
