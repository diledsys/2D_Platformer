using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerLadderClimber : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _climbSpeed = 4f;
    [SerializeField, Min(0f)] private float _verticalInputThreshold = 0.1f;
    [SerializeField, Min(0f)] private float _horizontalExitThreshold = 0.1f;
    [SerializeField, Min(0f)] private float _reattachDelay = 0.25f;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private PlayerLadderDetector _ladderDetector;
    [SerializeField] private PlayerGroundChecker _groundChecker;

    private Rigidbody2D _rigidbody;
    private float _defaultGravityScale;
    private float _detachUntilTime;

    public bool IsClimbing { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _defaultGravityScale = _rigidbody.gravityScale;

        if (_inputReader == null)
            _inputReader = GetComponent<PlayerInputReader>();

        if (_groundChecker == null)
            _groundChecker = GetComponent<PlayerGroundChecker>();
    }

    private void FixedUpdate()
    {
        float verticalInput = _inputReader.Move.y;
        float horizontalInput = _inputReader.Move.x;

        bool wantsToClimb = Mathf.Abs(verticalInput) > _verticalInputThreshold;
        bool wantsToLeaveSideways = Mathf.Abs(horizontalInput) > _horizontalExitThreshold;
        bool isGrounded = _groundChecker != null && _groundChecker.IsGrounded;

        if (IsClimbing)
        {
            if (_ladderDetector == null || _ladderDetector.IsInsideLadder == false)
            {
                ExitClimbMode();
                return;
            }

            if (isGrounded && wantsToLeaveSideways && wantsToClimb == false)
            {
                ExitClimbMode();
                return;
            }

            Vector2 velocity = _rigidbody.linearVelocity;
            velocity.x = 0f;
            velocity.y = wantsToClimb ? verticalInput * _climbSpeed : 0f;
            _rigidbody.linearVelocity = velocity;

            return;
        }

        if (Time.time < _detachUntilTime)
            return;

        if (_ladderDetector == null || _ladderDetector.IsInsideLadder == false)
            return;

        if (wantsToClimb == false)
            return;

        EnterClimbMode();
    }

    public void DetachByJump()
    {
        ExitClimbMode();
        _detachUntilTime = Time.time + _reattachDelay;
    }

    private void OnDisable()
    {
        _rigidbody.gravityScale = _defaultGravityScale;
        IsClimbing = false;
    }

    private void EnterClimbMode()
    {
        IsClimbing = true;
        _rigidbody.gravityScale = 0f;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void ExitClimbMode()
    {
        if (IsClimbing == false)
            return;

        IsClimbing = false;
        _rigidbody.gravityScale = _defaultGravityScale;
    }
}