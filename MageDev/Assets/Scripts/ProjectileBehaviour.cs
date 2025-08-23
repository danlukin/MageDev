using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private float projSpeed = 10f;
    [SerializeField] private LayerMask destroysProj;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetStraightVelocity();
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((destroysProj.value & (1 << collision.gameObject.layer)) > 0)
        {

            Destroy(gameObject);
            
        }
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * projSpeed;
    }


}
