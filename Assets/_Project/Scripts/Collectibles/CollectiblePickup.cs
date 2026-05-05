using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class CollectiblePickup : MonoBehaviour
{
    [SerializeField] private CollectibleDefinition _definition;
    [SerializeField, Min(1)] private int _amount = 1;
    [SerializeField] private LayerMask _collectorMask;

    private bool _isCollected;

    public event Action<CollectiblePickup> Collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
            return;

        GameObject collectorObject = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.transform.root.gameObject;

        if (IsInMask(collectorObject.layer) == false)
            return;

        PlayerCollectibleCollector collector =
            collectorObject.GetComponent<PlayerCollectibleCollector>();

        if (collector == null)
            return;

        _isCollected = true;
        collector.Collect(_definition, _amount);

        Collected?.Invoke(this);
        Destroy(gameObject);
    }

    public CollectibleDefinition Definition => _definition;

    private bool IsInMask(int layer)
    {
        return ( _collectorMask.value & ( 1 << layer ) ) != 0;
    }
}