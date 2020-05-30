using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : Singleton<PlayerController>
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
    private Block _targetBlock;
    private float _holdTime = 0.0f;
    private int _samples = 0;
    private Vector2 _sumTargetingDirection = new Vector2(0, 0);

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
        input.started +=
            context =>
            {
                if (card != null && card.Value.CanTarget && context.interaction is SlowTapInteraction)
                {
                    OnTarget(context, card);
                }
            };

        input.performed +=
            context =>
            {
                if (card != null)
                {
                    OnPlayCard(context, card);
                }
            };

        // Is this necessary if Tap gets priority?
        input.canceled +=
            context =>
            {
                if (card != null && context.interaction is SlowTapInteraction)
                {
                    OnPlayCard(context, card);
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

                // TODO Select next tile

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

    public void OnPlayCard(InputAction.CallbackContext context, CardInfo? card)
    {
        IsAnchored = false;
        Debug.Log("Playing card " + card);
    }

    public void OnTarget(InputAction.CallbackContext context, CardInfo? card)
    {
        IsAnchored = true;
        _holdTime = 0.0f;
        _samples = 0;
        _sumTargetingDirection = new Vector2();
        Debug.Log("Targeting with card " + card);
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
}
