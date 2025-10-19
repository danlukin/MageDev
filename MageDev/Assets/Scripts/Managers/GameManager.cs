using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public static GameState State;

    public static bool isPaused;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<bool> OnGamePause;
    

    void Awake()
    {
        Instance = this;
        isPaused = false;
    }

    void OnEnable()
    {
        TravelButton.OnTravel += UpdateGameState;
    }

        void OnDisable()
    {
        TravelButton.OnTravel -= UpdateGameState;
    }

    void Start()
    {
        UpdateGameState(GameState.Stage);
    }

    private void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Stage:
                
                break;
            case GameState.StageWin:
                HandleStageWin();
                break;
            case GameState.Camp:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleStage()
    {
        throw new NotImplementedException();
    }

    private void HandleStageWin()
    {
        Debug.Log("youre winner!");
    }

    public static void TogglePause()
    {
        isPaused = !isPaused;
        OnGamePause?.Invoke(isPaused);
    }
}

public enum GameState
{
    Stage,
    StageWin,
    Camp
}