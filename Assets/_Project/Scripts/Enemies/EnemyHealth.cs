using System;
using UnityEngine;

public sealed class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)] private int _maxHealth = 3;

    private int _currentHealth;
    private bool _isDead;

    public int CurrentHealth => _currentHealth;
    public event Action Died;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
            return;

        if (damage <= 0)
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        Debug.Log($"{name} took {damage} damage. HP = {_currentHealth}", this);

        if (_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        Died?.Invoke();

        Destroy(gameObject);
    }
}