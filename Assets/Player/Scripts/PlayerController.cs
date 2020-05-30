using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rigidBody => GetComponent<Rigidbody>();

    [ReadOnly]
    [SerializeField]
    [Tooltip("Used for debugging purposes only; not sampled in FixedUpdate")]
    private Vector2 MovementVector;

    public float MinimumMovementThresholdl = 0.05f;

    public float MovementSpeed = 1.0f;
    public float JumpSpeed = 1.0f;

    public bool IsGrounded = true;

    void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Move.started += ctx => MovementVector = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Move.performed += ctx => MovementVector = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Jump.started += OnJump;
        _playerInputActions.Player.PlayCard1.started += OnPlayCard1;
        _playerInputActions.Player.PlayCard2.started += OnPlayCard2;
        _playerInputActions.Player.PlayCard3.started += OnPlayCard3;
        _playerInputActions.Player.Escape.started += OnEscape;
    }

    void FixedUpdate()
    {
        Vector2 movementVector = ApplyThresholding(_playerInputActions.Player.Move.ReadValue<Vector2>());
        Vector3 velocity = new Vector3(movementVector.x, 0, movementVector.y) * MovementSpeed;
        
        // Respect velocity from jumping
        velocity.y = _rigidBody.velocity.y;

        _rigidBody.velocity = velocity;
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


    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");

        if (IsGrounded)
        {
            Vector3 impulse = new Vector3(0, 1, 0) * JumpSpeed;
            _rigidBody.AddForce(impulse, ForceMode.Impulse);
            IsGrounded = false;
        }
    }

    public void OnPlayCard1(InputAction.CallbackContext context)
    {
        Debug.Log("PlayCard1");
    }

    public void OnPlayCard2(InputAction.CallbackContext context)
    {
        Debug.Log("PlayCard2");
    }

    public void OnPlayCard3(InputAction.CallbackContext context)
    {
        Debug.Log("PlayCard3");
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        Debug.Log("Escape");
    }
}
