using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemsManager : MonoBehaviour
{
    [SerializeField] private ExpGem expGem;
    [SerializeField] private GoldCoin goldCoin;

    private List<GameObject> allDrops;

    private void OnEnable()
    {
        allDrops ??= new List<GameObject>();

        Enemy.OnEnemyKilled += EnemyOnKilled;

        GoldCoin.OnGoldDrop += HandleCurrencyDrop;
        GoldCoin.OnGoldPickUp += HandlePickUp;

        ExpGem.OnExpDrop += HandleCurrencyDrop;
        ExpGem.OnExpPickUp += HandlePickUp;
    }

    private void OnDisable() {
        Enemy.OnEnemyKilled -= EnemyOnKilled;

        GoldCoin.OnGoldDrop -= HandleCurrencyDrop;
        GoldCoin.OnGoldPickUp -= HandlePickUp;
        
        ExpGem.OnExpDrop -= HandleCurrencyDrop;
        ExpGem.OnExpPickUp -= HandlePickUp;
    }

    private void HandleCurrencyDrop(GameObject drop)
    {
        allDrops.Add(drop);
        drop.transform.SetParent(gameObject.transform, true);
    }

    private void HandlePickUp(GameObject drop)
    {
        allDrops.Remove(drop);
    }

    private void EnemyOnKilled(Enemy enemy)
    {
        expGem.SpawnExperienceGem(enemy);
        goldCoin.RandomGoldDrop(enemy);
    }
}
