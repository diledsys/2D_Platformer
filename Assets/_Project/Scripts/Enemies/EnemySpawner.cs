using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<EnemySpawnPoint> _spawnPoints = new();

    [SerializeField, Min(1)] private int _maxAliveEnemies = 3;
    [SerializeField, Min(0f)] private float _spawnInterval = 3f;
    [SerializeField] private bool _spawnOnStart = true;

    private readonly HashSet<EnemyDeathNotifier> _aliveEnemies = new();
    private WaitForSeconds _spawnDelay;
    private Coroutine _spawnRoutine;

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
            TrySpawnEnemy();
            yield return _spawnDelay;
        }
    }

    private void TrySpawnEnemy()
    {
        CleanupDestroyedEnemies();

        if (_enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: Enemy prefab is not assigned.", this);
            return;
        }

        if (_spawnPoints.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No spawn points assigned.", this);
            return;
        }

        if (_aliveEnemies.Count >= _maxAliveEnemies)
            return;

        EnemySpawnPoint spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint == null)
            return;

        GameObject enemyObject = Instantiate(
            _enemyPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation);

        EnemyDeathNotifier deathNotifier = enemyObject.GetComponent<EnemyDeathNotifier>();

        if (deathNotifier == null)
        {
            Debug.LogWarning("EnemySpawner: Spawned enemy has no EnemyDeathNotifier.", enemyObject);
            return;
        }

        deathNotifier.Died += OnEnemyDied;
        _aliveEnemies.Add(deathNotifier);
    }

    private EnemySpawnPoint GetRandomSpawnPoint()
    {
        if (_spawnPoints.Count == 0)
            return null;

        int index = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[index];
    }

    private void OnEnemyDied(EnemyDeathNotifier deathNotifier)
    {
        if (deathNotifier == null)
            return;

        deathNotifier.Died -= OnEnemyDied;
        _aliveEnemies.Remove(deathNotifier);
    }

    private void CleanupDestroyedEnemies()
    {
        _aliveEnemies.RemoveWhere(item => item == null);
    }
}