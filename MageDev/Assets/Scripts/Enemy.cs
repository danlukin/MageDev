using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] public float maxHealth = 3f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxAngle = 15;

    public Difficulty difficulty = Difficulty.normal;
    private Rigidbody2D rb;
    private FloatingHealthBar healthBar;
    private Transform target;
    private Vector2 moveDirection;
    private Vector3 scale;
    private float spawnTime;
    private float spawnImmuneTime = 0.1f;

    // damage to player
    private IDamageable playerCollision;
    [SerializeField] private float damageInterval = 0.5f;
    private float timeSinceDamageDealt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        EnemyManager.DestroyEnemy += EnemyManagerDestroyEnemy;
        spawnTime = Time.time;
    }

    void OnDestroy()
    {
        EnemyManager.DestroyEnemy -= EnemyManagerDestroyEnemy;
    }

    private void EnemyManagerDestroyEnemy(EnemyManager state)
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        scale = transform.localScale;
        SetDifficulty();

        maxHealth *= (float)Math.Pow(1.33f, StageManager.stageDifficulty);
        currentHealth = maxHealth;
        // currently only checks on start pls fix
        target = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            HandleMovement();
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    private void SetDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.normal:
                break;
            case Difficulty.elite:
                maxHealth *= 2;
                scale = new Vector3(scale.x * 1.5f, scale.y * 1.5f, scale.z * 1.5f);
                transform.localScale = scale;
                break;
            case Difficulty.boss:
                maxHealth *= 10;
                damage *= 2;
                scale = new Vector3(scale.x * 3, scale.y * 3, scale.z * 3);
                transform.localScale = scale;
                break;
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > maxAngle)
        {
            rb.rotation = maxAngle;
        }
        else if (angle < -maxAngle)
        {
            rb.rotation = -maxAngle;
        }
        else
        {
            rb.rotation = angle;
        }

        moveDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - timeSinceDamageDealt > damageInterval)
        {
            DealDamage(collision);
        }

        timeSinceDamageDealt = Time.time;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - timeSinceDamageDealt > damageInterval)
        {
            DealDamage(collision);
        }
    }

    private void DealDamage(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollision ??= collision.gameObject.GetComponent<IDamageable>();
            playerCollision.Damage(damage);
            timeSinceDamageDealt += damageInterval;
        }
    }

    public void Damage(float damageAmount)
    {
        if (Time.time - spawnTime > spawnImmuneTime)
        {
            if (currentHealth - damageAmount < 0) {currentHealth = 0;}
            else {currentHealth -= damageAmount;}
        
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                OnEnemyKilled?.Invoke(this);
            }
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

public enum Difficulty
{
    normal,
    elite,
    boss
}