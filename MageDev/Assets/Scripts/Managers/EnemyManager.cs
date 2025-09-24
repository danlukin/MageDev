using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    private int activeEnemyCount = 0;
    [SerializeField] private Enemy enemy;
    public static event Action<EnemyManager> DestroyEnemy;
    public static event Action<EnemyManager> OnWaveCompleted;
    private WaveState waveState;

    void Awake()
    {
        StageManager.OnWaveStateChanged += StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled += EnemyOnKilled;
    }

    void OnDestroy()
    {
        StageManager.OnWaveStateChanged -= StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled -= EnemyOnKilled;
    }

    void FixedUpdate()
    {
        if (activeEnemyCount == 0 & waveState != WaveState.Dead)
        {
            OnWaveCompleted?.Invoke(this);
        }
    }

    private void StageManagerOnWaveStateChanged(WaveState state)
    {
        waveState = state;

        switch (state)
        {
            case WaveState.WaveStart:
                {
                    SpawnNewWave();
                    break;
                }
            case WaveState.Dead:
                DestroyEnemy?.Invoke(this);
                activeEnemyCount = 0;
                break;
        }
    }

    private void SpawnNewWave()
    {
        int normalSpawn = (StageManager.stageDifficulty / 2) + 3;
        int eliteSpawn = StageManager.stageDifficulty / 3;

        for (int i = 0; i < normalSpawn; i++)
        { SpawnNewEnemy(Difficulty.normal); }

        for (int i = 0; i < eliteSpawn; i++)
        { SpawnNewEnemy(Difficulty.elite); }

        if (StageManager.waveNumber % 10 == 0)
        { SpawnNewEnemy(Difficulty.boss); }
    }

    private void SpawnNewEnemy(Difficulty difficulty)
    {
        var position = new Vector3(Random.Range(-9, 9), Random.Range(-9, 9));
        Quaternion noRotation = Quaternion.Euler(0, 0, 0);
        Enemy spawnedEnemy = Instantiate(enemy, position, noRotation);
        spawnedEnemy.difficulty = difficulty;
        ++activeEnemyCount;
    }

        private void EnemyOnKilled(Enemy enemy)
    {
        --activeEnemyCount;
    }
}
