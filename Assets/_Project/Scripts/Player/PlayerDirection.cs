using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public sealed class PlayerDirection : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _directionThreshold = 0.01f;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private Transform _visualRoot;

    private float _baseScaleX;
    public int FacingSign { get; private set; } = 1;

    private void Awake()
    {
        if (_inputReader == null)
            _inputReader = GetComponent<PlayerInputReader>();

        if (_visualRoot == null)
        {
            Debug.LogError("PlayerDirection: VisualRoot is not assigned.", this);
            enabled = false;
            return;
        }

        _baseScaleX = Mathf.Abs(_visualRoot.localScale.x);
    }

    private void Update()
    {
        if (_inputReader == null)
            return;

        float horizontalInput = _inputReader.Move.x;

        if (horizontalInput > _directionThreshold)
        {
            FacingSign = 1;
            FaceRight();
        }
        else if (horizontalInput < -_directionThreshold)
        {
            FacingSign = -1;
            FaceLeft();
        }
    }

    private void FaceRight()
    {
        Vector3 scale = _visualRoot.localScale;
        scale.x = _baseScaleX;
        _visualRoot.localScale = scale;
    }

    private void FaceLeft()
    {
        Vector3 scale = _visualRoot.localScale;
        scale.x = -_baseScaleX;
        _visualRoot.localScale = scale;
    }
}