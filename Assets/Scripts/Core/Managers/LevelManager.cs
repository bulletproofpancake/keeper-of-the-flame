using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Spawner")]
    [SerializeField] private GameObject[] levels;

    [Header("Player Spawn")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private GameObject _player;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemySpawnPoints[] enemySpawnPoints;
    public List<GameObject> enemies = new List<GameObject>();

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
        //GameManager.Instance.OnGameEnd += UnloadLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= LoadLevel;
        _player.GetComponent<PlayerMovement>().OnGemObtain -= ChangeLevel;
        //GameManager.Instance.OnGameEnd -= UnloadLevel;
    }

    private void LoadLevel()
    {
        // Despawns everything in the game to make sure that there are no duplicates
        UnloadLevel();
        
        levels[0].SetActive(true);
        levels[1].SetActive(false);
        
        SpawnPlayer();
        SpawnEnemies(0);
        SpawnGem();
        
        _player.GetComponent<PlayerMovement>().OnGemObtain += () =>
        {
            SpawnEnemies(1);
            SpawnDoor();
        };
    }

    public void UnloadLevel()
    {
        DespawnPlayer();
        DespawnEnemies();
        DespawnGem();
        DespawnDoor();
    }

    private void ChangeLevel()
    {
        print("Changing Level");
        levels[0].SetActive(false);
        levels[1].SetActive(true);
    }
    
    private void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        _player.GetComponent<PlayerMovement>().OnGemObtain += ChangeLevel;
        vCam.m_Follow = _player.transform;
    }

    private void SpawnEnemies(int level)
    {
        // foreach (var spawnPoint in enemySpawnPoints)
        // {
        //     var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, Quaternion.identity);
        //     _enemies.Add(enemy);
        // }

        foreach (var point in enemySpawnPoints[level].points)
        {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], point.position, Quaternion.identity);
        }
        
        // Relocates existing enemies to new spawn points
        for (int i = 0; i < enemySpawnPoints[level].points.Length; i++)
        {
            enemies[i].transform.position = enemySpawnPoints[level].points[i].position;
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
        foreach (var enemy in enemies)
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
