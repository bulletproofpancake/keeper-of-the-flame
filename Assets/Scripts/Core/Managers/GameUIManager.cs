using System;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("In Game Display")]
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private TextMeshProUGUI playerLivesCount;
    [SerializeField] private TextMeshProUGUI timerCountDisplay;
    
    [Header("End Game Display")]
    [SerializeField] private GameObject GameEndCanvas;
    [SerializeField] private TextMeshProUGUI GameEndText;
    
    private PlayerCombat _playerCombat;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () =>
        {
            playerCanvas.SetActive(true);
            GameEndCanvas.SetActive(false);
        };
        
        GameManager.Instance.OnGameEnd += () =>
        {
            playerCanvas.SetActive(false);
            GameEndCanvas.SetActive(true);
            GameOver();
        };
    }
    
    private void Start()
    {
        playerCanvas.SetActive(false);
        GameEndCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart) return;
        _playerCombat = FindObjectOfType<PlayerCombat>();
        playerLivesCount.text = $"x {_playerCombat.Health}";
        var timespan = TimeSpan.FromSeconds(GameManager.Instance.Timer);
        timerCountDisplay.text = $"{(int) timespan.TotalMinutes}:{timespan.Seconds:00}";
    }

    private void GameOver()
    {
        if (GameManager.Instance.playerWon)
        {
            GameEndText.text = "Congratulations";
            GameEndText.color = Color.yellow;
        }
        else
        {
            GameEndText.text = "Game Over";
            GameEndText.color = Color.red;
        }
    }
    
}
