using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Player Spawn")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private GameObject _player;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;
    private List<GameObject> _enemies = new List<GameObject>();

    [Header("Gem Spawn")]
    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private Transform gemSpawnPoint;
    private GameObject _gem;
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += LoadLevel;
        GameManager.Instance.OnGameEnd += UnloadLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= LoadLevel;
        GameManager.Instance.OnGameEnd -= UnloadLevel;
    }

    private void LoadLevel()
    {
        SpawnPlayer();
        SpawnEnemies();
        SpawnGem();
    }

    private void UnloadLevel()
    {
        DespawnPlayer();
        DespawnEnemies();
        DespawnGem();
    }
    
    private void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        vCam.m_Follow = _player.transform;
    }

    private void SpawnEnemies()
    {
        foreach (var spawnPoint in enemySpawnPoints)
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    private void SpawnGem()
    {
        _gem = Instantiate(gemPrefab, gemSpawnPoint.position, Quaternion.identity);
    }
    
    private void DespawnPlayer()
    {
        Destroy(_player);
    }

    private void DespawnEnemies()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
    }

    private void DespawnGem()
    {
        Destroy(_gem);
    }
    
}
