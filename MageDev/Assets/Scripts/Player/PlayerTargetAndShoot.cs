using System;
using System.Collections;
using UnityEngine;

public class PlayerTargetAndShoot : MonoBehaviour, IPausable
{

    [SerializeField] private GameObject spellObject;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float castRange;

    private GameObject[] allTargets;
    private GameObject target;
    private Vector2 targetPosition;
    private Vector2 weaponPosition;
    private Vector2 direction;

    private bool canCast = true;
    private float castSpeed;
    private float timeSinceCast;

    private void OnEnable()
    {
        PlayerSpellManager.OnCastRangeUpdated += HandleCastRangeUpdate;
        GameManager.OnGamePause += HandlePause;
    }

    private void OnDisable()
    {
        PlayerSpellManager.OnCastRangeUpdated -= HandleCastRangeUpdate;
        GameManager.OnGamePause -= HandlePause;
    }

    private void Start()
    {
        castSpeed = PlayerSpellManager.basicSpell.castSpeed;
        castRange = PlayerSpellManager.basicSpell.castRange;
    }

    private void FixedUpdate()
    {
        if (canCast & Time.time - timeSinceCast > castSpeed)
        {
            HandleTargeting();
        }
    }

    private void HandleTargeting()
    {
        allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (allTargets.Length > 0)
        {
            target = allTargets[0];
            foreach (GameObject tempTarget in allTargets)
            {
                if (Vector2.Distance(transform.position, tempTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                {
                    target = tempTarget;
                }
            }
            if (Vector2.Distance(transform.position, target.transform.position) < castRange)
            {
                Cast(target);
            }
        }
    }

    private void HandleWeaponRotation(GameObject target)
    {
        targetPosition = target.GetComponent<Transform>().position;
        weaponPosition = weapon.transform.position;
        direction = (targetPosition - weaponPosition).normalized;
        weapon.transform.right = direction;
    }

    private void Cast(GameObject target)
    {
        HandleWeaponRotation(target);
        Instantiate(spellObject, spawnPoint.position, weapon.transform.rotation);
        timeSinceCast = Time.time;
    }

    private void HandleCastRangeUpdate(PlayerSpellStats stats)
    {
        castRange = stats.castRange;
    }

    public void HandlePause(bool isPaused)
    {
        canCast = !isPaused;
    }

}


