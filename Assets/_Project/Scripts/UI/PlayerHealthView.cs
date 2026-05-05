using TMPro;
using UnityEngine;

public sealed class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        if (_text == null)
            _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (_playerHealth != null)
            _playerHealth.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnHealthChanged(int current, int max)
    {
        _text.text = $"HP: {current}/{max}";
    }

    private void Refresh()
    {
        if (_playerHealth == null || _text == null)
            return;

        _text.text = $"HP: {_playerHealth.CurrentHealth}/{_playerHealth.MaxHealth}";
    }
}