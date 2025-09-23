using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject spellSprite;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform projectileSpawnPoint;
    GameObject spellInstance;
    private Vector3 scale;
    private Vector3 originalScale;
    private bool offCooldown = true;
    private float timeSinceCast;

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
            spellInstance = Instantiate(spellSprite, projectileSpawnPoint.position, weapon.transform.rotation);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (offCooldown)
        {
            Pressed = false;
            Destroy(spellInstance);
            spellInstance = Instantiate(spell, projectileSpawnPoint.position, weapon.transform.rotation);
            spellInstance.GetComponent<PlayerProjectile>().transform.localScale = scale;

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
            if (scale.x < originalScale.x * 10)
            {
                scale = new Vector3(scale.x * 1.05f, scale.y * 1.05f, scale.z);
                spellInstance.transform.localScale = scale;
            }
        }
    }

}
