using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class EnemyMover : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _moveSpeed = 3f;

    private Rigidbody2D _rigidbody;

    public float CurrentMoveInput { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetMoveInput(float moveInput)
    {
        CurrentMoveInput = Mathf.Clamp(moveInput, -1f, 1f);
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.linearVelocity;
        velocity.x = CurrentMoveInput * _moveSpeed;
        _rigidbody.linearVelocity = velocity;
    }
}