using TMPro;
using UnityEngine;

public sealed class CollectibleCounterView : MonoBehaviour
{
    [SerializeField] private CollectibleInventory _inventory;
    [SerializeField] private CollectibleDefinition _definition;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        if (_text == null)
            _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (_inventory != null)
            _inventory.AmountChanged += OnAmountChanged;
    }

    private void OnDisable()
    {
        if (_inventory != null)
            _inventory.AmountChanged -= OnAmountChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnAmountChanged(CollectibleDefinition changedDefinition, int amount)
    {
        if (changedDefinition != _definition)
            return;

        Refresh();
    }

    private void Refresh()
    {
        if (_inventory == null || _definition == null || _text == null)
            return;

        int amount = _inventory.GetAmount(_definition);
        _text.text = $"{_definition.DisplayName}: {amount}";
    }
}