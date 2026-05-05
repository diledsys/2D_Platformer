using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions _actions;
    private Vector2 _move;
    private bool _isSprintHeld;

    public Vector2 Move => _move;
    public bool IsSprintHeld => _isSprintHeld;

    public event Action JumpPressed;
    public event Action AttackPressed;

    private void Awake()
    {
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _actions.Player.Move.performed += OnMovePerformed;
        _actions.Player.Move.canceled += OnMoveCanceled;
        _actions.Player.Sprint.performed += OnSprintPerformed;
        _actions.Player.Sprint.canceled += OnSprintCanceled;

        _actions.Player.Jump.performed += OnJumpPerformed;
        _actions.Player.Attack.performed += OnAttackPerformed;

        _actions.Enable();
    }

    private void OnDisable()
    {
        if (_actions == null)
            return;

        _actions.Player.Move.performed -= OnMovePerformed;
        _actions.Player.Move.canceled -= OnMoveCanceled;

        _actions.Player.Sprint.performed -= OnMovePerformed;
        _actions.Player.Sprint.canceled -= OnMoveCanceled;

        _actions.Player.Jump.performed -= OnJumpPerformed;
        _actions.Player.Attack.performed -= OnAttackPerformed;

        _actions.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _move = Vector2.zero;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        _isSprintHeld = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _isSprintHeld = false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpPressed?.Invoke();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AttackPressed?.Invoke();
    }
}