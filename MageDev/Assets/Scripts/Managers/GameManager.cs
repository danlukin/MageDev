using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Stage);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Stage:
                //HandleStage();
                break;
            case GameState.StageWin:
                HandleStageWin();
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
}

public enum GameState
{
    Stage,
    StageWin
}