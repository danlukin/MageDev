using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public enum SpellType
    {
        Basic,
        Super
    }

    [Header("Base values")]
    [SerializeField] private SpellType spellType;
    [SerializeField] private StatusEffectData status;
    [SerializeField] private float initialDamage;
    [SerializeField] private float initialProjSpeed;
    [SerializeField] private float initialCastSpeed;
    [SerializeField] private float initialCastRange;
    [SerializeField] private float initialStatusChance;

    [Header("Active values")]
    public float damage;
    public float baseDamage;
    public float damageMultiplier = 1;
    public float projSpeed;
    public float castSpeed;
    public float castRange;
    public float statusChance;

    public bool collisionEnabled = true;

    [SerializeField] private LayerMask collidesWith;
    [SerializeField] private LayerMask effects;
    private float destroyTime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        InitializeSpell();
    }

    private void InitializeSpell()
    {
        InitializeStats();
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        Destroy(gameObject, destroyTime);
    }

    private void InitializeStats()
    {
        baseDamage = initialDamage;
        UpdateDamage();
        projSpeed = initialProjSpeed;
        castSpeed = initialCastSpeed;
        castRange = initialCastRange;
        statusChance = initialStatusChance;
    }

    public void UpdateDamage()
    {
        damage = baseDamage * damageMultiplier;
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
            if ((collidesWith.value & (1 << collision.gameObject.layer)) > 0)
            {
                switch (spellType)
                {
                    case SpellType.Basic:
                        HandleCollision(collision);
                        if (UnityEngine.Random.value <= statusChance) HandleEffect(collision);
                        Destroy(gameObject);
                        break;
                    case SpellType.Super:
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
        iDamageable?.Damage(damage);
    }

    private void HandleEffect(Collider2D collision)
    {
        IEffectable iEffectable = collision.gameObject.GetComponent<IEffectable>();
        iEffectable?.ApplyEffect(status);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * projSpeed;
    }
}
