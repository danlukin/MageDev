using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject spellSprite;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform projectileSpawnPoint;
    private GameObject[] allTargets;
    private GameObject target;
    private GameObject spellInstance;
    private Vector3 scale;
    private Vector3 originalScale;
    private Vector2 direction;
    private bool offCooldown = true;
    private float timeSinceCast;
    private float chargeMultiplier = 1;

    void Start()
    {
        originalScale = spellSprite.transform.localScale;
        scale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (offCooldown)
        {
            Pressed = true;
            HandleTargeting();
            spellInstance = Instantiate(spellSprite, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (offCooldown)
        {
            Pressed = false;
            Destroy(spellInstance);
            spellInstance = Instantiate(spell, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
            spellInstance.transform.right = direction;
            spellInstance.GetComponent<PlayerProjectile>().transform.localScale = scale;
            spellInstance.GetComponent<PlayerProjectile>().projDamage *= chargeMultiplier * 0.2f;

            chargeMultiplier = 1;
            timeSinceCast = Time.time;
            scale = originalScale;
            offCooldown = false;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - timeSinceCast > spell.GetComponent<PlayerProjectile>().castRate)
        {
            offCooldown = true;
        }

        if (Pressed & offCooldown)
        {
            HandleTargeting();
            spellInstance.transform.right = direction;
            spellInstance.transform.parent = projectileSpawnPoint;
            if (chargeMultiplier < 50)
            {
                scale = new Vector3(scale.x * 1.05f, scale.y * 1.05f, scale.z);
                spellInstance.transform.localScale = scale;
                ++chargeMultiplier;
            }
        }
    }

    private void HandleTargeting()
    {
        allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (allTargets.Length > 0)
        {
            GameObject boss = Array.Find(allTargets, enemy => enemy.GetComponent<Enemy>().difficulty == Difficulty.boss);
            if (boss) { target = boss; }
            else
            {
                GameObject[] elite = Array.FindAll(allTargets, enemy => enemy.GetComponent<Enemy>().difficulty == Difficulty.elite);
                if (elite.Length > 0)
                {
                    FindClosest(elite);
                }
                else
                {
                    FindClosest(allTargets);
                }
            }
        }

        direction = (target.transform.position - projectileSpawnPoint.position).normalized;
    }

    private void FindClosest(GameObject[] list)
    {
        target = list[0];
        foreach (GameObject current in list)
        {
            if (Vector2.Distance(projectileSpawnPoint.transform.position, current.transform.position) < Vector2.Distance(projectileSpawnPoint.transform.position, target.transform.position))
            { target = current; }
        }
    }

}
