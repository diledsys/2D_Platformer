using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerLadderDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _ladderMask;

    private readonly HashSet<Collider2D> _ladders = new();

    public bool IsInsideLadder => _ladders.Count > 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInMask(other.gameObject.layer) == false)
            return;

        _ladders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInMask(other.gameObject.layer) == false)
            return;

        _ladders.Remove(other);
    }

    private void OnDisable()
    {
        _ladders.Clear();
    }

    private bool IsInMask(int layer)
    {
        return ( _ladderMask.value & ( 1 << layer ) ) != 0;
    }
}