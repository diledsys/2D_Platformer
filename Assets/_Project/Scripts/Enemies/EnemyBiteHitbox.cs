using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class EnemyBiteHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D _hitboxCollider;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField, Min(1)] private int _damage = 1;

    private readonly HashSet<IDamageable> _damagedTargets = new();

    private void Awake()
    {
        if (_hitboxCollider == null)
            _hitboxCollider = GetComponent<Collider2D>();

        _hitboxCollider.enabled = false;
    }

    private void OnDisable()
    {
        _damagedTargets.Clear();

        if (_hitboxCollider != null)
            _hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInMask(other.gameObject.layer) == false)
            return;

        TryDamage(other);
    }

    public void BeginAttackWindow()
    {
        _damagedTargets.Clear();
        _hitboxCollider.enabled = true;
        ApplyDamageToCurrentOverlaps();
    }

    public void EndAttackWindow()
    {
        _hitboxCollider.enabled = false;
        _damagedTargets.Clear();
    }

    private void ApplyDamageToCurrentOverlaps()
    {
        ContactFilter2D filter = new();
        filter.useLayerMask = true;
        filter.layerMask = _targetMask;
        filter.useTriggers = true;

        List<Collider2D> results = new();
        _hitboxCollider.Overlap(filter, results);

        foreach (Collider2D result in results)
            TryDamage(result);
    }

    private void TryDamage(Collider2D other)
    {
        IDamageable damageable = FindDamageable(other);

        if (damageable == null)
            return;

        if (_damagedTargets.Add(damageable) == false)
            return;

        damageable.TakeDamage(_damage);
    }

    private IDamageable FindDamageable(Collider2D other)
    {
        MonoBehaviour[] behaviours = other.GetComponentsInParent<MonoBehaviour>(true);

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IDamageable damageable)
                return damageable;
        }

        return null;
    }

    private bool IsInMask(int layer)
    {
        return ( _targetMask.value & ( 1 << layer ) ) != 0;
    }
}