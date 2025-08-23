using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPoint;

    private GameObject projectileIntance;

    private Vector2 worldPosiotion;
    private Vector2 direction;

    void Start()
    {

    }

    void Update()
    {
        HandleWeaponRotation();
        HandleWeaponShooting();
    }

    private void HandleWeaponRotation()
    {
        worldPosiotion = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosiotion - (Vector2)weapon.transform.position).normalized;
        weapon.transform.right = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Vector3 localScale = weapon.transform.localScale;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -10f;
        }
        else
        {
            localScale.y = 10f;
        }

        weapon.transform.localScale = localScale;
    }

    private void HandleWeaponShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            projectileIntance = Instantiate(projectile, projectileSpawnPoint.position, weapon.transform.rotation);

        }
    }
}
