using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyTargetDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;

    private readonly HashSet<Rigidbody2D> _targets = new();

    public Transform CurrentTarget
    {
        get
        {
            foreach (Rigidbody2D target in _targets)
            {
                if (target != null)
                    return target.transform;
            }

            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInMask(other.gameObject.layer) == false)
            return;

        Rigidbody2D targetRigidbody = other.attachedRigidbody;

        if (targetRigidbody == null)
            return;

        _targets.Add(targetRigidbody);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInMask(other.gameObject.layer) == false)
            return;

        Rigidbody2D targetRigidbody = other.attachedRigidbody;

        if (targetRigidbody == null)
            return;

        _targets.Remove(targetRigidbody);
    }

    private void OnDisable()
    {
        _targets.Clear();
    }

    private bool IsInMask(int layer)
    {
        return ( _playerMask.value & ( 1 << layer ) ) != 0;
    }
}