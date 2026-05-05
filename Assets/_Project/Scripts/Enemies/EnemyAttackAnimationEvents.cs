using UnityEngine;

public sealed class EnemyAttackAnimationEvents : MonoBehaviour
{
    [SerializeField] private EnemyBiteHitbox _biteHitbox;

    public void BeginAttackWindow()
    {
        if (_biteHitbox != null)
            _biteHitbox.BeginAttackWindow();
    }

    public void EndAttackWindow()
    {
        if (_biteHitbox != null)
            _biteHitbox.EndAttackWindow();
    }
}