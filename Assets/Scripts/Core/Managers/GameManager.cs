using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private bool _isGameStart;
    private bool _isGameEnd;
    private float _timer;
    
    public bool playerWon;

    public bool IsGameStart => _isGameStart;
    public bool IsGameEnd => _isGameEnd;
    public float Timer => _timer;
    
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public void StartGame()
    {
        OnGameStart?.Invoke();
        _timer = 0f;
        _isGameStart = true;
        _isGameEnd = false;
    }
    
    private void Update()
    {
        if (_isGameStart)
        {
            _timer += Time.deltaTime;
        }
    }

    public void GameEnd(bool isPlayerWin)
    {
        OnGameEnd?.Invoke();
        _isGameStart = false;
        _isGameEnd = true;
        playerWon = isPlayerWin;
    }

}
