using System;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    
    [Header("In Game Instructions")]
    [SerializeField] private GameObject instructionsCanvas;
    [SerializeField] private bool firstRun = true;

    [Header("In Game Display")]
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private TextMeshProUGUI playerLivesCount;
    [SerializeField] private TextMeshProUGUI timerCountDisplay;
    
    [Header("End Game Display")]
    [SerializeField] private GameObject GameEndCanvas;
    [SerializeField] private TextMeshProUGUI GameEndText;
    [SerializeField] private TextMeshProUGUI TimerDisplay;
    
    private PlayerCombat _playerCombat;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () =>
        {
            playerCanvas.SetActive(true);
            
            if(firstRun) 
            { instructionsCanvas.SetActive(true); }
            
            GameEndCanvas.SetActive(false);
        };
        
        GameManager.Instance.OnGameEnd += () =>
        {
            playerCanvas.SetActive(false);
            instructionsCanvas.SetActive(false);
            GameEndCanvas.SetActive(true);
            GameOver();
        };
    }
    
    private void Start()
    {
        playerCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);
        GameEndCanvas.SetActive(false);
        AudioManager.Instance.Play("bgm");
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart) return;
        _playerCombat = FindObjectOfType<PlayerCombat>();
        playerLivesCount.text = $"x {_playerCombat.Health}";
        timerCountDisplay.text = $"{(int) GameManager.Instance.Span.TotalMinutes}:{GameManager.Instance.Span.Seconds:00}";
    }

    private void GameOver()
    {
        firstRun = false;
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
        TimerDisplay.text = $"You ran for {timerCountDisplay.text}";
    }
    
}
