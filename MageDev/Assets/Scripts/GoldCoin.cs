using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoldCoin : MonoBehaviour
{
    [SerializeField] private int baseGold = 1;
    private int gold;

    public static event Action<GameObject> OnGoldDrop;
    public static event Action<GameObject> OnGoldPickUp;

    private void Awake()
    {
        gold = baseGold;
    }

    void OnDestroy()
    {
        OnGoldPickUp?.Invoke(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCurrency.HandleCurrencyChange(Currency.Gold, gold);
            Destroy(gameObject);
        }
    }

    public void RandomGoldDrop(Enemy enemy)
    {
        bool dropGold = false;

        float normalDropRate = 0.01f;
        float eliteDropRate = 0.02f;
        float bossDropRate = 1;

        switch (enemy.difficulty)
        {
            case Difficulty.normal:
                if (Random.Range(0f, 1f) <= normalDropRate) dropGold = true;
                break;
            case Difficulty.elite:
                if (Random.Range(0f, 1f) <= eliteDropRate) dropGold = true;
                break;
            case Difficulty.boss:
                if (Random.Range(0f, 1f) <= bossDropRate) dropGold = true;
                break;
        }
        
        if (dropGold) SpawnGoldCoin(enemy);
    }

    public void SpawnGoldCoin(Enemy enemy)
    {
        Vector3 spawnPosition = enemy.transform.position;
        GoldCoin newCoin = Instantiate(this, spawnPosition, Quaternion.identity);

        OnGoldDrop?.Invoke(newCoin.gameObject);
    }
}
