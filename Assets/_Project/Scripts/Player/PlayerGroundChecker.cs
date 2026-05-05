using UnityEngine;

public sealed class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField, Min(0f)] private float _checkRadius = 0.1f;
    [SerializeField] private LayerMask _groundMask;

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            _groundCheckPoint.position,
            _checkRadius,
            _groundMask) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _checkRadius);
    }
}