using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class PlayerAnimationPresenter : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private static readonly int IsClimbingHash = Animator.StringToHash("IsClimbing");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int DamageHash = Animator.StringToHash("Damage");
    private static readonly int DiedHash = Animator.StringToHash("Died");

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private PlayerGroundChecker _groundChecker;
    [SerializeField] private PlayerLadderClimber _ladderClimber;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField, Min(0f)] private float _moveThreshold = 0.01f;


    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_rigidbody == null)
            _rigidbody = GetComponentInParent<Rigidbody2D>();

        if (_inputReader == null)
            _inputReader = GetComponentInParent<PlayerInputReader>();

        if (_groundChecker == null)
            _groundChecker = GetComponentInParent<PlayerGroundChecker>();

        if (_ladderClimber == null)
            _ladderClimber = GetComponentInParent<PlayerLadderClimber>();

        if (_playerAttack == null)
            _playerAttack = GetComponentInParent<PlayerAttack>();

        if (_playerJump == null)
            _playerJump = GetComponentInParent<PlayerJump>();

        if (_playerHealth == null)
            _playerHealth = GetComponentInParent<PlayerHealth>();

        ValidateDependencies();
    }

    private void OnEnable()
    {
        if (_playerAttack != null)
            _playerAttack.AttackPerformed += OnAttackPerformed;

        if (_playerJump != null)
            _playerJump.JumpStarted += OnJumpStarted;

        if (_playerHealth != null)
            _playerHealth.DamageTaken += OnDamageTaken;

        if (_playerHealth != null)
            _playerHealth.Died += OnDied;
    }


    private void OnDisable()
    {
        if (_playerAttack != null)
            _playerAttack.AttackPerformed -= OnAttackPerformed;

        if (_playerJump != null)
            _playerJump.JumpStarted -= OnJumpStarted;

        if (_playerHealth != null)
        {
            _playerHealth.Died -= OnDied;
            _playerHealth.DamageTaken -= OnDamageTaken;
        }

    }

    private void Update()
    {
        if (_animator == null || _rigidbody == null || _inputReader == null)
            return;

        float horizontalInput = Mathf.Abs(_inputReader.Move.x);
        bool hasMoveInput = horizontalInput > _moveThreshold;
        float animationSpeed = 0f;

        if (hasMoveInput)
            animationSpeed = _inputReader.IsSprintHeld ? 1f : 0.5f;

        _animator.SetFloat(SpeedHash, horizontalInput);
        _animator.SetFloat(SpeedHash, animationSpeed);
        _animator.SetFloat(VerticalSpeedHash, _rigidbody.linearVelocity.y);
        _animator.SetBool(IsClimbingHash, _ladderClimber != null && _ladderClimber.IsClimbing);

        if (_groundChecker != null)
            _animator.SetBool(IsGroundedHash, _groundChecker.IsGrounded);
    }

    private void OnAttackPerformed()
    {
        if (_animator == null)
            return;

        _animator.SetTrigger(AttackHash);
    }

    private void ValidateDependencies()
    {
        if (_animator == null)
            Debug.LogError("PlayerAnimationPresenter: Animator not found.", this);

        if (_rigidbody == null)
            Debug.LogError("PlayerAnimationPresenter: Rigidbody2D not found in parents.", this);

        if (_inputReader == null)
            Debug.LogError("PlayerAnimationPresenter: PlayerInputReader not found in parents.", this);
    }

    private void OnJumpStarted()
    {
        _animator.ResetTrigger(JumpHash);
        _animator.SetTrigger(JumpHash);
    }
    private void OnDamageTaken()
    {
        _animator.SetTrigger(DamageHash);
    }
    private void OnDied()
    {
        _animator.SetTrigger(DiedHash);
    }
}