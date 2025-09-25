using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;

    public static event Action<PlayerHealth> OnPlayerKilled;

    void Awake()
    {
        StageManager.OnWaveStateChanged += StageManagerOnWaveStateChanged;
    }

    void OnDestroy()
    {
        StageManager.OnWaveStateChanged -= StageManagerOnWaveStateChanged;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            OnPlayerKilled?.Invoke(this);
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void StageManagerOnWaveStateChanged(WaveState state)
    {
        if (state == WaveState.WaveStart)
        {
            Heal(maxHealth);
        }
    }
}
