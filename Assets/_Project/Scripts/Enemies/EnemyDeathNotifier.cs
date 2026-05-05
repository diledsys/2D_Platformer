using System;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public sealed class EnemyDeathNotifier : MonoBehaviour
{
    private EnemyHealth _enemyHealth;

    public event Action<EnemyDeathNotifier> Died;

    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        if (_enemyHealth != null)
            _enemyHealth.Died += OnEnemyDied;
    }

    private void OnDisable()
    {
        if (_enemyHealth != null)
            _enemyHealth.Died -= OnEnemyDied;
    }

    private void OnEnemyDied()
    {
        Died?.Invoke(this);
    }
}