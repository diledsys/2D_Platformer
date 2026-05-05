using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class EnemyAnimationPresenter : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int BiteHash = Animator.StringToHash("Bite");

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private EnemyAttack _enemyAttack;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_rigidbody == null)
            _rigidbody = GetComponentInParent<Rigidbody2D>();

        if (_enemyAttack == null)
            _enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnEnable()
    {
        if (_enemyAttack != null)
            _enemyAttack.AttackStarted += OnAttackStarted;
    }

    private void OnDisable()
    {
        if (_enemyAttack != null)
            _enemyAttack.AttackStarted -= OnAttackStarted;
    }

    private void Update()
    {
        if (_animator == null || _rigidbody == null)
            return;

        _animator.SetFloat(SpeedHash, Mathf.Abs(_rigidbody.linearVelocity.x));
    }

    private void OnAttackStarted()
    {
        _animator.ResetTrigger(BiteHash);
        _animator.SetTrigger(BiteHash);
    }
}