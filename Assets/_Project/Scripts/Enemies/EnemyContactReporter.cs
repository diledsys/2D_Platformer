using UnityEngine;

public sealed class EnemyContactReporter : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInMask(collision.gameObject.layer) == false)
            return;

        Debug.Log($"Enemy touched player: {collision.gameObject.name}", this);
    }

    private bool IsInMask(int layer)
    {
        return ( _playerMask.value & ( 1 << layer ) ) != 0;
    }
}