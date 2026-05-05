using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public sealed class EnemyDirection : MonoBehaviour
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private Transform _visualPivot;
    [SerializeField, Min(0f)] private float _directionThreshold = 0.01f;

    private float _baseScaleX;

    private void Awake()
    {
        if (_enemyMover == null)
            _enemyMover = GetComponent<EnemyMover>();

        if (_visualPivot == null)
        {
            Debug.LogError("EnemyDirection: VisualPivot is not assigned.", this);
            enabled = false;
            return;
        }

        _baseScaleX = Mathf.Abs(_visualPivot.localScale.x);
    }

    private void Update()
    {
        float moveInput = _enemyMover.CurrentMoveInput;

        if (moveInput > _directionThreshold)
            FaceLeft();
        else if (moveInput < -_directionThreshold)
            FaceRight();
    }

    private void FaceRight()
    {
        Vector3 scale = _visualPivot.localScale;
        scale.x = _baseScaleX;
        _visualPivot.localScale = scale;
    }

    private void FaceLeft()
    {
        Vector3 scale = _visualPivot.localScale;
        scale.x = -_baseScaleX;
        _visualPivot.localScale = scale;
    }
}