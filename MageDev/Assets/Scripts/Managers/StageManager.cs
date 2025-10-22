using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{

    public static StageManager Instance;
    public WaveState waveState;
    public static event Action<WaveState> OnWaveStateChanged;
    public static int waveNumber;
    public static int stageDifficulty = 0;

    [SerializeField] TMP_Text WaveUI;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject relicSelectUI;
    [SerializeField] GameObject relicInventory;

    [SerializeField] private int checkpoint = 1;
    private bool waveChanged = false;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerKilled += PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted += EnemyManagerOnWaveCompleted;

        InitializeStage();
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerKilled -= PlayerHealthOnPlayerKilled;
        EnemyManager.OnWaveCompleted -= EnemyManagerOnWaveCompleted;

        UpdateWaveState(WaveState.NotStage);
    }

    private void InitializeStage()
    {
        waveChanged = false;
        WaveUI.gameObject.SetActive(true);
        UpdateWaveNumber("reset");
        UpdateWaveUI();
        UpdateWaveState(WaveState.WaveStart);

        if (relicInventory.transform.childCount == 0) relicSelectUI.SetActive(true);
    }

    public void UpdateWaveState(WaveState newState)
    {
        waveState = newState;
        
        if (deathScreen != null) deathScreen.SetActive(newState == WaveState.Dead);
        if (winScreen != null) winScreen.SetActive(newState == WaveState.StageComplete);

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
                StartCoroutine(Respawn());
                break;
            case WaveState.WaveStart:
                waveChanged = false;
                break;
            case WaveState.StageComplete:
                break;
            case WaveState.NotStage:
                if (WaveUI != null) WaveUI.gameObject.SetActive(false);
                break;
        }

        OnWaveStateChanged?.Invoke(newState);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        UpdateWaveState(WaveState.WaveStart);
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
    StageComplete,
    NotStage
}
