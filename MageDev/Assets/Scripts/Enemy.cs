using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    // damage to player
    private IDamageable playerCollision;
    [SerializeField] private float damageInterval = 0.5f;
    private float timeSinceDamageDealt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        EnemyManager.DestroyEnemy += EnemyManagerDestroyEnemy;
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

    private void HandleMovement()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > 30)
        {
            rb.rotation = 30;
        }
        else if (angle < -30)
        {
            rb.rotation = -30;
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

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            OnEnemyKilled?.Invoke(this);
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
