using System;
using UnityEngine;

public sealed class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)] private int _maxHealth = 5;

    private int _currentHealth;
    private bool _isDead;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    public event Action<int, int> HealthChanged;
    public event Action Died;
    public event Action DamageTaken;

    private void Start()
    {
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

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
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        Debug.Log($"Player took {damage} damage. HP = {_currentHealth}", this);

        DamageTaken?.Invoke();
        
        if (_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        Debug.Log("Player died", this);
        Died?.Invoke();
    }

    public void Heal(int amount)
    {
        if (_isDead)
            return;

        if (amount <= 0)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}