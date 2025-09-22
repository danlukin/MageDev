using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private float projSpeed;
    [SerializeField] private float projDamage;
    [SerializeField] public float castRate;
    [SerializeField] private LayerMask destroysProj;

    public enum ProjectileType
    {
        Fireball,
        GigaFireball
    }
    public ProjectileType projectileType;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, destroyTime);

        // Set stats based on projectile type
        SetProjectileStats();

        //InitializeProjectile();

    }

    private void FixedUpdate()
    {
        transform.right = rb.velocity;
    }

    private void InitializeProjectile()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((destroysProj.value & (1 << collision.gameObject.layer)) > 0)
        {

            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            iDamageable?.Damage(projDamage);

            Destroy(gameObject);

        }
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * projSpeed;
    }

    private void SetProjectileStats()
    {
        if (projectileType == ProjectileType.Fireball)
        {
            SetStraightVelocity();
            projDamage = 1f;
            projSpeed = 10f;
            castRate = 2f;
        }
    }

}
