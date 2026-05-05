using UnityEngine;

public sealed class PlayerCollectibleCollector : MonoBehaviour
{
    [SerializeField] private CollectibleInventory _inventory;

    public void Collect(CollectibleDefinition definition, int amount)
    {
        if (_inventory == null)
            return;

        _inventory.Add(definition, amount);
    }
}