using System;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{

    public static StageManager Instance;
    public WaveState waveState;
    public static event Action<WaveState> OnWaveStateChanged;
    public Text WaveUI;
    public static int waveNumber;
    public static int stageDifficulty = 0;

    [SerializeField] GameObject deathScreen;

    private bool waveChanged = false;

    void Awake()
    {
        Instance = this;
        PlayerHealth.OnPlayerKilled += PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted += EnemyManagerOnWaveCompleted;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerKilled -= PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted -= EnemyManagerOnWaveCompleted;
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
        waveNumber = 1;
        UpdateWaveUI();
        UpdateWaveState(WaveState.WaveStart);

    }

    public void UpdateWaveState(WaveState newState)
    {
        waveState = newState;

        deathScreen.SetActive(newState == WaveState.Dead);

        switch (newState)
        {
            case WaveState.WaveComplete:
                if (!waveChanged)
                {
                    UpdateWaveNumber("increment");
                    UpdateWaveState(WaveState.WaveStart);
                }
                break;
            case WaveState.Dead:
                UpdateWaveNumber("reset");
                break;
            case WaveState.WaveStart:
                waveChanged = false;
                break;
        }

        OnWaveStateChanged?.Invoke(newState);
    }

    private void EnemyManagerOnWaveCompleted(EnemyManager state)
    {
        UpdateWaveState(WaveState.WaveComplete);
    }

    private void PlayerHealthOnPlayerKilled(PlayerHealth state)
    {
        UpdateWaveState(WaveState.Dead);
    }

    private void UpdateWaveNumber(string task)
    {
        switch (task)
        {
            case "increment":
                ++waveNumber;
                HandleStageDifficulty();
                break;
            case "decrement":
                --waveNumber;
                HandleStageDifficulty();
                break;
            case "reset":
                waveNumber = 1;
                stageDifficulty = 0;
                break;
        }

        UpdateWaveUI();
        waveChanged = true;
    }

    private void UpdateWaveUI()
    {
        WaveUI.text = "Wave " + waveNumber;
    }

    private void HandleStageDifficulty()
    {
        if (waveNumber % 2 == 0)
        {
            ++stageDifficulty;
        }
    }

}

public enum WaveState
{
    WaveStart,
    Dead,
    WaveComplete
}
