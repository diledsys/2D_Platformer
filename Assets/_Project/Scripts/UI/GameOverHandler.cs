using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameOverHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField, Min(0f)] private float _gameOverDelay = 5f;

    private bool _isGameOver;

    private void OnEnable()
    {
        if (_playerHealth != null)
            _playerHealth.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.Died -= OnPlayerDied;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);
    }

    private void OnPlayerDied()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(_gameOverDelay);

        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}