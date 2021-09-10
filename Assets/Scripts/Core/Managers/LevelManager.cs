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
    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

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
        if(level == 0)
            DespawnEnemies();

        var points = enemySpawnPoints[level].points;

        for (int i = 0; i < points.Length; i++)
        {
            _enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], points[i].position, Quaternion.identity));
        }
        
        print(_enemies.Count + " | " + points.Length );
        
        if (_enemies.Count > points.Length)
        {
            for (int i = 0; i < points.Length; i++)
            {
                _enemies[i].transform.position = points[i].transform.position;
            }
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
        _enemies.Clear();
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
