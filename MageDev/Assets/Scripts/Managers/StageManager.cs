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
        PlayerHealth.OnPlayerKilled += PlayerHealthOnPlayerKilled;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerKilled -= PlayerHealthOnPlayerKilled;
    }

    void Start()
    {
        InitializeStage();
    }

    void Update()
    {
        if (waveState == WaveState.Dead)
        {
            if (Input.anyKey)
            {
                UpdateWaveState(WaveState.WaveStart);
            }
        }
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

    private void PlayerHealthOnPlayerKilled(PlayerHealth state)
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
