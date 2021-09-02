using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += SpawnCharacters;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= SpawnCharacters;
    }

    private void SpawnCharacters()
    {
        Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    }

}
