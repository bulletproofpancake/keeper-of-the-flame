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
    
    private void Update()
    {
        if (!_isGameStart) return;
        
        OnGameStart?.Invoke();
        print("Game Start");
        _isGameStart = false;
    }

    public void StartGame()
    {
        _isGameStart = true;
    }
    
}
