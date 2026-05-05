using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerMover : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _walkSpeed = 3f;
    [SerializeField, Min(0f)] private float _runSpeed = 6f;
    [SerializeField, Min(0f)] private float _moveThreshold = 0.01f;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private PlayerLadderClimber _ladderClimber;

    private float _horizontalControlLockUntilTime;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_inputReader == null)
            _inputReader = GetComponent<PlayerInputReader>();

        if (_ladderClimber == null)
            _ladderClimber = GetComponent<PlayerLadderClimber>();
    }

    private void FixedUpdate()
    {
        if (Time.time < _horizontalControlLockUntilTime)
            return;

        Vector2 velocity = _rigidbody.linearVelocity;

        if (_ladderClimber != null && _ladderClimber.IsClimbing)
        {
            velocity.x = 0f;
            _rigidbody.linearVelocity = velocity;
            return;
        }

        float horizontalInput = _inputReader.Move.x;
        bool hasMoveInput = Mathf.Abs(horizontalInput) > _moveThreshold;
        bool isRunning = hasMoveInput && _inputReader.IsSprintHeld;

        float currentSpeed = isRunning ? _runSpeed : _walkSpeed;

        velocity.x = horizontalInput * currentSpeed;
        _rigidbody.linearVelocity = velocity;
    }

    public void LockHorizontalControl(float duration)
    {
        _horizontalControlLockUntilTime = Mathf.Max(_horizontalControlLockUntilTime, Time.time + duration);
    }
}