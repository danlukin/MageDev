using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Currency
{
    Gold
}

public class PlayerCurrency : MonoBehaviour
{
    public static int gold;

    public static event Action OnGoldChanged;

    public static void HandleCurrencyChange(Currency currency, int amount)
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

