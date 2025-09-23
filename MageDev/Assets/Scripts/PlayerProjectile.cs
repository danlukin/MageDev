using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] public float projSpeed;
    [SerializeField] public float projDamage;
    [SerializeField] public float castRate;
    [SerializeField] public float castRange = 8f;
    public bool collisionEnabled = true;

    [SerializeField] private LayerMask destroysProj;

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
            if ((destroysProj.value & (1 << collision.gameObject.layer)) > 0)
            {
                switch (projectileType)
                {
                    case ProjectileType.Basic:
                        HandleBasicCollision(collision);
                        break;
                    case ProjectileType.Ultimate:
                        HandleUltimateCollision(collision);
                        break;
                }
            }
        }
    }

    private void HandleBasicCollision(Collider2D collision)
    {
        IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
        iDamageable?.Damage(projDamage);
        Destroy(gameObject);
    }

    private void HandleUltimateCollision(Collider2D collision)
    {
        IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
        iDamageable?.Damage(projDamage);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * projSpeed;
    }
}
