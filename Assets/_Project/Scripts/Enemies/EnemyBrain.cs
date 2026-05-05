using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public sealed class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyTargetDetector _targetDetector;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyAttack _enemyAttack;

    [SerializeField, Min(0f)] private float _stopDistance = 0.2f;
    [SerializeField, Range(0f, 1f)] private float _pauseChance = 0.25f;
    [SerializeField, Min(0f)] private float _minDecisionInterval = 1.2f;
    [SerializeField, Min(0f)] private float _maxDecisionInterval = 2.5f;
    [SerializeField, Min(0f)] private float _minPauseDuration = 0.4f;
    [SerializeField, Min(0f)] private float _maxPauseDuration = 1.2f;

    private float _nextDecisionTime;
    private float _pauseUntilTime;

    private void Awake()
    {
        if (_enemyMover == null)
            _enemyMover = GetComponent<EnemyMover>();

        if (_enemyAttack == null)
            _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if (_targetDetector == null)
        {
            _enemyMover.SetMoveInput(0f);
            return;
        }

        Transform target = _targetDetector.CurrentTarget;

        if (target == null)
        {
            _enemyMover.SetMoveInput(0f);
            return;
        }

        float deltaX = target.position.x - transform.position.x;
        float distance = Mathf.Abs(deltaX);

        if (_enemyAttack != null && distance <= _enemyAttack.AttackRange)
        {
            _enemyMover.SetMoveInput(0f);
            _enemyAttack.TryAttack(target, transform);
            return;
        }

        if (Time.time < _pauseUntilTime)
        {
            _enemyMover.SetMoveInput(0f);
            return;
        }

        if (Time.time >= _nextDecisionTime)
        {
            _nextDecisionTime = Time.time + Random.Range(_minDecisionInterval, _maxDecisionInterval);

            if (Random.value < _pauseChance)
            {
                _pauseUntilTime = Time.time + Random.Range(_minPauseDuration, _maxPauseDuration);
                _enemyMover.SetMoveInput(0f);
                return;
            }
        }

        if (distance <= _stopDistance)
        {
            _enemyMover.SetMoveInput(0f);
            return;
        }

        _enemyMover.SetMoveInput(Mathf.Sign(deltaX));
    }
}