using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private bool _isGameStart;
    private bool _isGameEnd;

    public bool playerWon;

    public bool IsGameStart => _isGameStart;
    public bool IsGameEnd => _isGameEnd;
    
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public void StartGame()
    {
        OnGameStart?.Invoke();
        _isGameStart = true;
    }

    public void GameEnd(bool isPlayerWin)
    {
        OnGameEnd?.Invoke();
        _isGameEnd = false;
        playerWon = isPlayerWin;
    }


}
