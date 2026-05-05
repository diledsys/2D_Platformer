using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerJump : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _jumpForce = 10f;
    [SerializeField, Min(0f)] private float _ladderJumpForce = 10f;
    [SerializeField, Min(0f)] private float _ladderJumpHorizontalSpeed = 7f;
    [SerializeField, Min(0f)] private float _ladderJumpControlLockTime = 0.18f;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private PlayerGroundChecker _groundChecker;
    [SerializeField] private PlayerLadderClimber _ladderClimber;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerDirection _playerDirection;

    private Rigidbody2D _rigidbody;
    private bool _isJumpRequested;
    private bool _isJumpLocked;

    public event Action JumpStarted;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_inputReader == null)
            _inputReader = GetComponent<PlayerInputReader>();

        if (_groundChecker == null)
            _groundChecker = GetComponent<PlayerGroundChecker>();

        if (_ladderClimber == null)
            _ladderClimber = GetComponent<PlayerLadderClimber>();

        if (_playerMover == null)
            _playerMover = GetComponent<PlayerMover>();

        if (_playerDirection == null)
            _playerDirection = GetComponent<PlayerDirection>();
    }

    private void OnEnable()
    {
        _inputReader.JumpPressed += OnJumpPressed;
    }

    private void OnDisable()
    {
        _inputReader.JumpPressed -= OnJumpPressed;
    }

    private void FixedUpdate()
    {
        UpdateJumpLockState();

        if (_isJumpRequested == false)
            return;

        _isJumpRequested = false;

        if (_ladderClimber != null && _ladderClimber.IsClimbing)
        {
            _ladderClimber.DetachByJump();
            ApplyLadderJump();
            return;
        }

        if (_isJumpLocked)
            return;

        if (_groundChecker == null || _groundChecker.IsGrounded == false)
            return;

        ApplyGroundJump();
    }

    private void OnJumpPressed()
    {
        if (_ladderClimber != null && _ladderClimber.IsClimbing)
        {
            _isJumpRequested = true;
            return;
        }

        if (_isJumpLocked)
            return;

        _isJumpRequested = true;
    }

    private void ApplyGroundJump()
    {
        Vector2 velocity = _rigidbody.linearVelocity;
        velocity.y = _jumpForce;
        _rigidbody.linearVelocity = velocity;

        _isJumpLocked = true;
        JumpStarted?.Invoke();
    }

    private void ApplyLadderJump()
    {
        int directionSign = GetLadderJumpDirection();

        Vector2 velocity = _rigidbody.linearVelocity;
        velocity.x = directionSign * _ladderJumpHorizontalSpeed;
        velocity.y = _ladderJumpForce;
        _rigidbody.linearVelocity = velocity;

        if (_playerMover != null)
            _playerMover.LockHorizontalControl(_ladderJumpControlLockTime);

        _isJumpLocked = true;
        JumpStarted?.Invoke();
    }

    private int GetLadderJumpDirection()
    {
        float horizontalInput = _inputReader.Move.x;

        if (horizontalInput > 0.1f)
            return 1;

        if (horizontalInput < -0.1f)
            return -1;

        if (_playerDirection != null)
            return _playerDirection.FacingSign;

        return 1;
    }

    private void UpdateJumpLockState()
    {
        if (_groundChecker == null)
            return;

        bool isLanded = _groundChecker.IsGrounded && _rigidbody.linearVelocity.y <= 0f;

        if (isLanded)
            _isJumpLocked = false;
    }
}