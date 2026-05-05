using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerAttack : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _attackCooldown = 0.35f;
    [SerializeField] private PlayerInputReader _inputReader;

    private float _nextAttackTime;

    public event Action AttackPerformed;

    private void Awake()
    {
        if (_inputReader == null)
            _inputReader = GetComponent<PlayerInputReader>();
    }

    private void OnEnable()
    {
        _inputReader.AttackPressed += OnAttackPressed;
    }

    private void OnDisable()
    {
        _inputReader.AttackPressed -= OnAttackPressed;
    }

    private void OnAttackPressed()
    {
        if (Time.time < _nextAttackTime)
            return;

        _nextAttackTime = Time.time + _attackCooldown;
        AttackPerformed?.Invoke();
    }
}