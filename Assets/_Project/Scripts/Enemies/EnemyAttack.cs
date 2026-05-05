using System;
using UnityEngine;

public sealed class EnemyAttack : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _attackRange = 0.8f;
    [SerializeField, Min(0f)] private float _attackCooldown = 1.2f;

    private float _nextAttackTime;

    public float AttackRange => _attackRange;
    public bool CanAttack => Time.time >= _nextAttackTime;

    public event Action AttackStarted;

    public bool TryAttack(Transform target, Transform self)
    {
        if (target == null || self == null)
            return false;

        if (CanAttack == false)
            return false;

        float distance = Mathf.Abs(target.position.x - self.position.x);

        if (distance > _attackRange)
            return false;

        _nextAttackTime = Time.time + _attackCooldown;
        AttackStarted?.Invoke();
        return true;
    }
}