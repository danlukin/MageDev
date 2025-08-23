using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public static StageManager Instance;
    public WaveState waveState;
    public int WaveNumber;
    public static event Action<WaveState> OnWaveStateChanged;

    [SerializeField] GameObject deathScreen;

    void Awake()
    {
        Instance = this;
        PlayerController.OnPlayerKilled += PlayerControllerOnPlayerKilled;
    }

    void OnDestroy()
    {
        PlayerController.OnPlayerKilled -= PlayerControllerOnPlayerKilled;
    }

    void Start()
    {
        InitializeStage();
    }

    private void InitializeStage()
    {
        WaveNumber = 1;
        UpdateWaveState(WaveState.WaveStart);

    }

    public void UpdateWaveState(WaveState newState)
    {
        waveState = newState;

        deathScreen.SetActive(newState == WaveState.Dead);

        switch (newState)
        {
            case WaveState.WaveEnd:
                break;
            case WaveState.Dead:
                break;
            case WaveState.WaveStart:
                break;
        }

        OnWaveStateChanged?.Invoke(newState);
    }

    private void PlayerControllerOnPlayerKilled(PlayerController controller)
    {
        UpdateWaveState(WaveState.Dead);
    }

}

public enum WaveState
{
    WaveStart,
    Dead,
    WaveEnd
}
