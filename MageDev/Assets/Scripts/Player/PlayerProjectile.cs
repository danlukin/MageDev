
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] public float projSpeed;
    [SerializeField] public float baseDamage;
    [SerializeField] public float castRate;
    [SerializeField] public float castRange = 8f;
    [SerializeField] public float statusChance = 0.1f;
    public bool collisionEnabled = true;

    [SerializeField] private LayerMask destroysProj;
    [SerializeField] private LayerMask effects;
    [SerializeField] public StatusEffectData data;

    public enum ProjectileType
    {
        Basic,
        Ultimate
    }
    public ProjectileType projectileType;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, destroyTime);

        SetStraightVelocity();

    }

    private void FixedUpdate()
    {
        transform.right = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionEnabled)
        {
            // change to be more clear
            if ((destroysProj.value & (1 << collision.gameObject.layer)) > 0)
            {
                switch (projectileType)
                {
                    case ProjectileType.Basic:
                        HandleCollision(collision);
                        if (UnityEngine.Random.value <= statusChance) HandleEffect(collision);
                        Destroy(gameObject);
                        break;
                    case ProjectileType.Ultimate:
                        HandleCollision(collision);
                        if (effects.value > 0) HandleEffect(collision);
                        break;
                }
            }
        }
    }

    private void HandleCollision(Collider2D collision)
    {
        IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
        iDamageable?.Damage(baseDamage);
    }

    private void HandleEffect(Collider2D collision)
    {
        IEffectable iEffectable = collision.gameObject.GetComponent<IEffectable>();
        //iEffectable?.ApplyEffect(data);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * projSpeed;
    }
}
