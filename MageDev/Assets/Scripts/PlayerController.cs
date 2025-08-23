using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;
    public Rigidbody2D body;
    public float speed;

    public static event Action<PlayerController> OnPlayerKilled;

    void Awake()
    {
        StageManager.OnWaveStateChanged += GameManagerOnWaveStateChanged;
    }

    void OnDestroy()
    {
        StageManager.OnWaveStateChanged -= GameManagerOnWaveStateChanged;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;
        body.velocity = direction * speed;

    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            speed = 0;
            OnPlayerKilled?.Invoke(this);
        }
    }
    
    private void GameManagerOnWaveStateChanged(WaveState state)
    {
        if (state == WaveState.Dead)
        {
            Debug.Log("ded");
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
}
