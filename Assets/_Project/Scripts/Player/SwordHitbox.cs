using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class SwordHitbox : MonoBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private Collider2D _hitboxCollider;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField, Min(1)] private int _damage = 1;
    [SerializeField, Min(0f)] private float _activeTime = 0.12f;

    private readonly HashSet<IDamageable> _damagedTargets = new();
    private Coroutine _disableRoutine;
    private WaitForSeconds _activeDelay;

    private void Awake()
    {
        if (_playerAttack == null)
            _playerAttack = GetComponentInParent<PlayerAttack>();

        if (_hitboxCollider == null)
            _hitboxCollider = GetComponent<Collider2D>();

        _hitboxCollider.enabled = false;
        _activeDelay = new WaitForSeconds(_activeTime);
    }

    private void OnEnable()
    {
        if (_playerAttack != null)
            _playerAttack.AttackPerformed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        if (_playerAttack != null)
            _playerAttack.AttackPerformed -= OnAttackPerformed;

        if (_disableRoutine != null)
            StopCoroutine(_disableRoutine);

        _disableRoutine = null;
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

    private void OnAttackPerformed()
    {
        _damagedTargets.Clear();

        if (_disableRoutine != null)
            StopCoroutine(_disableRoutine);

        _hitboxCollider.enabled = true;

        ApplyDamageToCurrentOverlaps();

        _disableRoutine = StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return _activeDelay;

        _hitboxCollider.enabled = false;
        _disableRoutine = null;
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