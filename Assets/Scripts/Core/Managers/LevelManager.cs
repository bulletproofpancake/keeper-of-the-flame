using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private GameObject _player;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += SpawnCharacters;
        GameManager.Instance.OnGameEnd += DespawnPlayer;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= SpawnCharacters;
        GameManager.Instance.OnGameEnd -= DespawnPlayer;
    }

    private void SpawnCharacters()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        vCam.m_Follow = _player.transform;
    }

    private void DespawnPlayer()
    {
        Destroy(_player);
    }
}
