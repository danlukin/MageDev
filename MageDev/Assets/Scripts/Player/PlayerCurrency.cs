using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    Gold
}

public class PlayerCurrency : MonoBehaviour
{
    public static float gold;

    public static event Action OnGoldChanged;

    public static void HandleCurrencyChange(Currency currency, float amount)
    {
        switch (currency)
        {
            case Currency.Gold:
                gold += amount;
                OnGoldChanged?.Invoke();
                break;
        }
    }
}

