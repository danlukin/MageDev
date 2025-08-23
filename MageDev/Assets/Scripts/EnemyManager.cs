using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    private int activeEnemyCount = 0;
    private GameObject enemyInstance;
    [SerializeField] private GameObject enemy;
    public static event Action<EnemyManager> DestroyEnemy;

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
        if (activeEnemyCount == 0)
        {
            SpawnNewWave();
        }
    }

    private void StageManagerOnWaveStateChanged(WaveState state)
    {
        switch (state)
        {
            case WaveState.WaveStart:
                {
                    SpawnNewWave();
                    break;
                }
            case WaveState.Dead:
                DestroyEnemy?.Invoke(this);
                break;
        }
    }

    private void SpawnNewWave()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        var position = new Vector3(Random.Range(-9, 9), Random.Range(-9, 9));
        Quaternion noRotation = Quaternion.Euler(0, 0, 0);
        enemyInstance = Instantiate(enemy, position, noRotation);
        ++activeEnemyCount;
    }

        private void EnemyOnKilled(Enemy enemy)
    {
        --activeEnemyCount;
    }
}
