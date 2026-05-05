using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CollectibleStackSpawner : MonoBehaviour
{
    [SerializeField] private CollectiblePickup _collectiblePrefab;
    [SerializeField, Min(1)] private int _maxAliveItems = 5;
    [SerializeField, Min(0f)] private float _spawnInterval = 3f;
    [SerializeField, Min(0f)] private float _stackStepY = 0.35f;
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private bool _restackAfterCollect = true;

    private readonly List<CollectiblePickup> _aliveItems = new();

    private Coroutine _spawnRoutine;
    private WaitForSeconds _spawnDelay;

    private void Awake()
    {
        _spawnDelay = new WaitForSeconds(_spawnInterval);
    }

    private void OnEnable()
    {
        if (_spawnOnStart)
            StartSpawning();
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = null;
    }

    [ContextMenu("Start Spawning")]
    public void StartSpawning()
    {
        if (_spawnRoutine != null)
            return;

        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    [ContextMenu("Stop Spawning")]
    public void StopSpawning()
    {
        if (_spawnRoutine == null)
            return;

        StopCoroutine(_spawnRoutine);
        _spawnRoutine = null;
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            TrySpawn();
            yield return _spawnDelay;
        }
    }

    private void TrySpawn()
    {
        CleanupDestroyedItems();

        if (_collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleStackSpawner: Collectible prefab is not assigned.", this);
            return;
        }

        if (_aliveItems.Count >= _maxAliveItems)
            return;

        Vector3 spawnPosition = transform.position + Vector3.up * ( _stackStepY * _aliveItems.Count );

        CollectiblePickup item = Instantiate(_collectiblePrefab, spawnPosition, Quaternion.identity);
        item.Collected += OnItemCollected;

        _aliveItems.Add(item);
    }

    private void OnItemCollected(CollectiblePickup item)
    {
        if (item == null)
            return;

        item.Collected -= OnItemCollected;
        _aliveItems.Remove(item);

        if (_restackAfterCollect)
            RestackAliveItems();
    }

    private void RestackAliveItems()
    {
        CleanupDestroyedItems();

        for (int i = 0; i < _aliveItems.Count; i++)
        {
            if (_aliveItems[i] == null)
                continue;

            Vector3 position = transform.position + Vector3.up * ( _stackStepY * i );
            _aliveItems[i].transform.position = position;
        }
    }

    private void CleanupDestroyedItems()
    {
        for (int i = _aliveItems.Count - 1; i >= 0; i--)
        {
            if (_aliveItems[i] == null)
                _aliveItems.RemoveAt(i);
        }
    }
}