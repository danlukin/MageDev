using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    [SerializeField] SpellType spellToCast;
    public PlayerSpellStats spell;

    [SerializeField] float dmg;

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
        SetSpellStats();
        gameObject.GetComponent<SpriteRenderer>().sprite = spell.sprite;
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        Destroy(gameObject, destroyTime);
    }

    private void OnDestroy()
    {
        if (spell.spellType == SpellType.Super & FindObjectOfType<PlayerSpellManager>())
        {
            PlayerSpellManager.SetChargeMultiplier(1);
        }
    }

    private void SetSpellStats()
    {
        switch (spellToCast)
        {
            case SpellType.Basic:
                spell = PlayerSpellManager.basicSpell;
                break;
            case SpellType.Super:
                spell = PlayerSpellManager.superSpell;
                break;
        }
    }

    private void FixedUpdate()
    {
        transform.right = rb.velocity;

        // for testing only
        dmg = spell.damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionEnabled)
        {
            // change to be more clear
            if ((collidesWith.value & (1 << collision.gameObject.layer)) > 0 & spell != null)
            {
                switch (spell.spellType)
                {
                    case SpellType.Basic:
                        HandleCollision(collision);
                        if (UnityEngine.Random.value <= spell.statusChance) HandleEffect(collision);
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
        iDamageable?.Damage(spell.damage);
    }

    private void HandleEffect(Collider2D collision)
    {
        IEffectable iEffectable = collision.gameObject.GetComponent<IEffectable>();
        iEffectable?.ApplyEffect(PlayerSpellManager.status);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * spell.projSpeed;
    }
}
