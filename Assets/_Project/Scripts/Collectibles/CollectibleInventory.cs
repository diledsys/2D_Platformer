using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CollectibleInventory : MonoBehaviour
{
    private readonly Dictionary<CollectibleDefinition, int> _amounts = new();

    public event Action<CollectibleDefinition, int> AmountChanged;

    public int GetAmount(CollectibleDefinition definition)
    {
        if (definition == null)
            return 0;

        return _amounts.TryGetValue(definition, out int amount) ? amount : 0;
    }

    public void Add(CollectibleDefinition definition, int amount)
    {
        if (definition == null || amount <= 0)
            return;

        int currentAmount = GetAmount(definition);
        int newAmount = currentAmount + amount;

        _amounts[definition] = newAmount;
        AmountChanged?.Invoke(definition, newAmount);

        Debug.Log($"{definition.DisplayName}: {newAmount}", this);
    }
}