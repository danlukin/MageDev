using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float minBoundsPadding;
    [SerializeField] private float maxBoundsPadding;
    [SerializeField] private Enemy[] enemies;
    
    public int activeEnemyCount = 0;
    
    public static event Action<EnemyManager> DestroyEnemy;
    public static event Action<EnemyManager> OnWaveCompleted;
    private WaveState waveState;

    //enemies
    private Enemy normalEnemy;
    private Enemy eliteEnemy;
    private Enemy bossEnemy;


    void Awake()
    {
        InstantializeEnemies();
    }

    void OnEnable()
    {
        StageManager.OnWaveStateChanged += StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled += EnemyOnKilled;

    }

    void OnDisable()
    {
        StageManager.OnWaveStateChanged -= StageManagerOnWaveStateChanged;
        Enemy.OnEnemyKilled -= EnemyOnKilled;

        ResetEnemyManager();
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
        Vector3 position = GetRandomPosition();
        Quaternion noRotation = Quaternion.Euler(0, 0, 0);
        Instantiate(enemy, position, noRotation);
        ++activeEnemyCount;
    }

    private float[] GetCameraBounds()
    {
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float boundPadding = Random.Range(minBoundsPadding, maxBoundsPadding);
        float leftBound = camPos.x - horzExtent - boundPadding;
        float rightBound = camPos.x + horzExtent + boundPadding;
        float bottomBound = camPos.y - vertExtent - boundPadding;
        float topBound = camPos.y + vertExtent + boundPadding;

        float[] bounds = {leftBound, rightBound, bottomBound, topBound};
        return bounds;
    }

    private Vector3 GetRandomPosition()
    {
        float[] bounds = GetCameraBounds();
        int bound = Random.Range(0, 4);

        if (bound < 2) return new Vector3(bounds[bound], Random.Range(bounds[2], bounds[3]));
        else return new Vector3(Random.Range(bounds[0], bounds[1]), bounds[bound]);
    }

    private void EnemyOnKilled(Enemy enemy)
    {
        --activeEnemyCount;
    }

    private void ResetEnemyManager()
    {
        DestroyEnemy?.Invoke(this);
        activeEnemyCount = 0;
    }
}
