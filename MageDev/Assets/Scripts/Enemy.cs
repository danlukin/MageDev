using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    normal,
    elite,
    boss
}

public class Enemy : MonoBehaviour, IDamageable, IEffectable
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] public float maxHealth = 3f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxAngle = 15;
    [SerializeField] private GameObject statusContainer;

    public Difficulty difficulty = Difficulty.normal;
    private Rigidbody2D rb;
    private FloatingHealthBar healthBar;
    private Transform target;
    private Vector2 moveDirection;
    private Vector3 scale;
    private float spawnTime;
    private float spawnImmuneTime = 0.1f;
    private StatusEffect status;

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
        if (status != null) HandleEffect();

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
            currentHealth -= damageAmount;

            healthBar.UpdateHealthBar(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                HandleOnDeath();
            }
        }
    }

    private void HandleOnDeath()
    {
        if (status != null) RemoveEffect();
        OnEnemyKilled?.Invoke(this);
        Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    [SerializeField] private float currentEffectTime;
    private float nextTickTime;

    public void ApplyEffect(StatusEffect _status)
    {
        if (currentHealth > 0)
        {
            if (status != null)
            {
                currentEffectTime = nextTickTime - currentEffectTime;
                if (currentEffectTime < 0) currentEffectTime = -currentEffectTime;
                nextTickTime = 0;
            }
            else
            {
                status = _status;
                HandleStatusAnimation(true);
            }

            status = _status;
        }

        
    }

    private void HandleStatusAnimation(bool effectActive)
    {
        statusContainer.transform.Find(status.name).gameObject.SetActive(effectActive);
    }

    public void HandleEffect()
    {
        currentEffectTime += Time.deltaTime;

        if (status != null & currentEffectTime >= status.duration) RemoveEffect();

        if (status == null) return;

        if (status.HOTAmount > 0 && currentEffectTime >= nextTickTime)
        {
            nextTickTime += status.tickRate;
            Heal(status.HOTAmount);
        }

        if (status.DOTAmount > 0 && currentEffectTime >= nextTickTime)
        {
            nextTickTime += status.tickRate;
            Damage(status.DOTAmount);
        }
    }

    public void RemoveEffect()
    {
        HandleStatusAnimation(false);
        status = null;
        currentEffectTime = 0;
        nextTickTime = 0;
    }
}

