using UnityEngine;

public class PlayerTargetAndShoot : MonoBehaviour
{

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float castRange;

    private GameObject[] allTargets;
    private GameObject target;
    private Vector2 targetPosition;
    private Vector2 weaponPosition;
    private Vector2 direction;

    private float castRate;
    private float timeSinceCast;
    private GameObject projectileIntance;

    private void Start()
    {
        castRate = projectile.GetComponent<PlayerProjectile>().castRate;
        castRange = projectile.GetComponent<PlayerProjectile>().castRange;
    }

    private void FixedUpdate()
    {
        if (Time.time - timeSinceCast > castRate)
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
        projectileIntance = Instantiate(projectile, projectileSpawnPoint.position, weapon.transform.rotation);
        timeSinceCast = Time.time;
    }
    
}


