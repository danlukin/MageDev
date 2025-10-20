using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;

    private FloatingHealthBar healthBar;

    public static event Action<PlayerHealth> OnPlayerKilled;

    void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();

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

        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnPlayerKilled?.Invoke(this);
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        healthBar.UpdateHealthBar(currentHealth, maxHealth);

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
