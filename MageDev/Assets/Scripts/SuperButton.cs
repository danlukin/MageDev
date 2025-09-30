using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SuperButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;
    [SerializeField] private GameObject spellObject;
    [SerializeField] private GameObject spellSprite;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform projectileSpawnPoint;

    private PlayerSpellStats spell;
    private GameObject[] allTargets;
    private GameObject target;
    private GameObject spellInstance;
    private Vector3 scale;
    private Vector3 originalScale;
    private Vector2 direction;
    private bool offCooldown = true;
    private float timeSinceCast;
    private float currentCharge = 1;

    void Start()
    {
        originalScale = spellSprite.transform.localScale;
        scale = originalScale;
        spell = PlayerSpellManager.superSpell;
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
            spellInstance = Instantiate(spellObject, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
            spellInstance.transform.right = direction;
            
            spellInstance.transform.localScale = scale;
            PlayerSpellManager.SetChargeMultiplier(spell, currentCharge * 0.2f);

            currentCharge = 1;
            timeSinceCast = Time.time;
            scale = originalScale;
            offCooldown = false;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - timeSinceCast > spell.castSpeed)
        {
            gameObject.GetComponent<Image>().fillAmount = (Time.time - timeSinceCast) / spell.castSpeed;
            offCooldown = true;
        }

        if (!offCooldown)
        {
            gameObject.GetComponent<Image>().fillAmount = (Time.time - timeSinceCast) / spell.castSpeed;
        }

        if (Pressed & offCooldown)
        {
            HandleTargeting();
            spellInstance.transform.right = direction;
            spellInstance.transform.parent = projectileSpawnPoint;

            if (currentCharge < spell.maxChargeStacks)
            {
                scale = new Vector3(scale.x * 1.05f, scale.y * 1.05f, scale.z);
                spellInstance.transform.localScale = scale;
                ++currentCharge;
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
