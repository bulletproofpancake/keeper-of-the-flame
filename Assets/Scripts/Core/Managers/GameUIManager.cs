using System;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private TextMeshProUGUI playerLivesCount;
    [SerializeField] private TextMeshProUGUI timerCountDisplay;
    private PlayerCombat _playerCombat;


    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () => playerCanvas.SetActive(true);
    }
    
    private void Start()
    {
        playerCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart) return;
        _playerCombat = FindObjectOfType<PlayerCombat>();
        playerLivesCount.text = $"x {_playerCombat.Health}";
        var timespan = TimeSpan.FromSeconds(GameManager.Instance.Timer);
        timerCountDisplay.text = $"{(int) timespan.TotalMinutes}:{timespan.Seconds:00}";
    }
}
