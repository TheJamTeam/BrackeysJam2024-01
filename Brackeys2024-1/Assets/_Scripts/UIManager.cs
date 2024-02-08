using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverWinLossText;
    public TextMeshProUGUI gameOverScore;
    
    private void OnEnable()
    {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameOver -= OnGameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameOver += OnGameOver;
    }
    
    void OnGameOver()
    {
        menu.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverWinLossText.text = GameStateManager.Instance.GetGameState().isVictorious ? "Victory" : "Defeat";
        gameOverScore.text = GameStateManager.Instance.GetGameState().score.ToString("00.00");
    }
    
}
