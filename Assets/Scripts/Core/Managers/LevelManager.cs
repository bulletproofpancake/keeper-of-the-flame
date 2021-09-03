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

    [Header("Exit Spawn")]
    [SerializeField] private GameObject doorPrefab;
    private GameObject _door;
    
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
        // Despawns everything in the game to make sure that there are no duplicates
        Despawner();
        
        SpawnPlayer();
        SpawnEnemies();
        SpawnGem();
        
        _player.GetComponent<PlayerMovement>().OnGemObtain += () =>
        {
            SpawnEnemies();
            SpawnDoor();
        };
    }

    private void UnloadLevel()
    {
        // Delayed so that death animations would play
        // If player dies
        Invoke("Despawner",3f);
    }

    private void Despawner()
    {
        DespawnPlayer();
        DespawnEnemies();
        DespawnGem();
        DespawnDoor();
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

    private void SpawnDoor()
    {
        _door = Instantiate(doorPrefab, playerSpawn.position, Quaternion.identity);
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

    private void DespawnDoor()
    {
        Destroy(_door);
    }
    
}
