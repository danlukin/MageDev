using System;
using TMPro;
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
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject relicSelectUI;

    [SerializeField] TMP_Text goldCountText;
    [SerializeField] TMP_Text goldInventoryText;

    [SerializeField] private int checkpoint = 1;
    private bool waveChanged = false;

    void Awake()
    {
        Instance = this;
        PlayerHealth.OnPlayerKilled += PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted += EnemyManagerOnWaveCompleted;
        PlayerCurrency.OnGoldChanged += UpdateGoldUI;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerKilled -= PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted -= EnemyManagerOnWaveCompleted;
        PlayerCurrency.OnGoldChanged -= UpdateGoldUI;
    }

    void Start()
    {
        InitializeStage();
        UpdateGoldUI();
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
        relicSelectUI.SetActive(true);
    }

    public void UpdateWaveState(WaveState newState)
    {
        waveState = newState;

        deathScreen.SetActive(newState == WaveState.Dead);
        winScreen.SetActive(newState == WaveState.StageComplete);

        switch (newState)
        {
            case WaveState.WaveComplete:
                if (!waveChanged)
                {
                    //if (CheckForWin()) { UpdateWaveState(WaveState.StageComplete); }
                    //else
                    //{
                        UpdateWaveNumber("increment");
                        UpdateWaveState(WaveState.WaveStart);
                    //}
                }
                break;
            case WaveState.Dead:
                UpdateWaveNumber("checkpoint");
                break;
            case WaveState.WaveStart:
                waveChanged = false;
                break;
            case WaveState.StageComplete:
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

    private void HandleCheckpointReached()
    {
        checkpoint = waveNumber;
        relicSelectUI.SetActive(true);
    }

    private void UpdateWaveNumber(string task)
    {
        switch (task)
        {
            case "increment":
                ++waveNumber;
                CheckForWin();
                HandleStageDifficulty();
                if (waveNumber % 10 == 1) HandleCheckpointReached();
                break;
            case "decrement":
                --waveNumber;
                HandleStageDifficulty();
                break;
            case "reset":
                waveNumber = 1;
                stageDifficulty = 0;
                break;
            case "checkpoint":
                int difference = waveNumber - checkpoint;
                if (difference == 1)
                {
                    difference *= 2;
                }
                stageDifficulty -= difference / 2;
                waveNumber = checkpoint;
                break;
        }

        UpdateWaveUI();
        waveChanged = true;
    }

    private bool CheckForWin()
    {
        return waveNumber == 20;
    }

    private void UpdateWaveUI()
    {
        WaveUI.text = "Wave " + waveNumber;
    }

    private void UpdateGoldUI()
    {
        goldCountText.text = PlayerCurrency.gold.ToString();
        goldInventoryText.text = PlayerCurrency.gold.ToString();
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
    WaveComplete,
    StageComplete
}
