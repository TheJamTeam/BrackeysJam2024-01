using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameState
{
    public string name;
    public float score;
    public bool isVictorious;
    public float playTime;
}

public class GameStateManager : Singleton<GameStateManager>
{
    [field: SerializeField]
    public bool IsPaused { get; private set; }

    private GameState gameState;
    
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action<bool> OnGamePauseChanged;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    //The game entry point
    private void StartGame()
    {
        if(IsPaused || Time.timeScale == 0) ResumeGame();
        gameState = new GameState();
        
        OnGameStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused) return;
        GameUpdate();
    }

    void GameUpdate()
    {
        //Connect components here for update. This makes sure that different components run in the correct order.
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
        OnGamePauseChanged?.Invoke(true);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
        OnGamePauseChanged?.Invoke(false);
    }

    public void GameOver(bool isVictory)
    {
        if (isVictory)
        {
            UpdateGameState(null, 0, 0, true); 
        }
        
        PauseGame();
        OnGameOver?.Invoke();
    }

    public void UpdateGameState(string name=null, float scoreToAdd=0, float playTimeToAdd=0, bool isVictorious=false)
    {
        if (name != null)
        {
            gameState.name = name;
        }

        if (scoreToAdd > 0)
        {
            gameState.score += scoreToAdd;
        }

        if (playTimeToAdd > 0)
        {
            gameState.playTime += playTimeToAdd;
        }

        if (isVictorious)
        {
            gameState.isVictorious = true;
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

}
