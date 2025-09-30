using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private int activeEnemyCount = 0;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private ExpGem expGem;
    public static event Action<EnemyManager> DestroyEnemy;
    public static event Action<EnemyManager> OnWaveCompleted;
    private WaveState waveState;

    //enemies
    private Enemy normalEnemy;
    private Enemy eliteEnemy;
    private Enemy bossEnemy;


    void Awake()
    {
        StageManager.OnWaveStateChanged += StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled += EnemyOnKilled;

        InstantializeEnemies();
    }

    void OnDestroy()
    {
        StageManager.OnWaveStateChanged -= StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled -= EnemyOnKilled;
    }

    void FixedUpdate()
    {
        if (activeEnemyCount <= 0 & waveState != WaveState.Dead)
        {
            OnWaveCompleted?.Invoke(this);
        }
    }

    private void InstantializeEnemies()
    {
        normalEnemy = enemies[0];
        normalEnemy.difficulty = Difficulty.normal;
        eliteEnemy = enemies[1];
        eliteEnemy.difficulty = Difficulty.elite;
        bossEnemy = enemies[2];
        bossEnemy.difficulty = Difficulty.boss;
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
        int normalSpawn = (StageManager.stageDifficulty / 2) + 5;
        int eliteSpawn = StageManager.stageDifficulty / 2;

        for (int i = 0; i < normalSpawn; i++)
        { SpawnNewEnemy(normalEnemy); }

        for (int i = 0; i < eliteSpawn; i++)
        { SpawnNewEnemy(eliteEnemy); }

        if (StageManager.waveNumber % 10 == 0)
        { SpawnNewEnemy(bossEnemy); }
    }

    private void SpawnNewEnemy(Enemy enemy)
    {
        var position = new Vector3(Random.Range(-9, 9), Random.Range(-9, 9));
        Quaternion noRotation = Quaternion.Euler(0, 0, 0);
        Instantiate(enemy, position, noRotation);
        ++activeEnemyCount;
    }

    private void EnemyOnKilled(Enemy enemy)
    {
        expGem.SpawnExperienceGem(enemy);
        --activeEnemyCount;
    }
}
