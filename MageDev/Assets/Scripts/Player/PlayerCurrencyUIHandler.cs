using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCurrencyUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text goldCountText;
    [SerializeField] private TMP_Text goldInventoryText;

    void Awake()
    {
        UpdateGoldUI();
    }

    void OnEnable()
    {
        PlayerCurrency.OnGoldChanged += UpdateGoldUI;
    }

    void OnDisable()
    {
        PlayerCurrency.OnGoldChanged -= UpdateGoldUI;
    }

    public void UpdateGoldUI()
    {
        goldCountText.text = PlayerCurrency.gold.ToString();
        goldInventoryText.text = PlayerCurrency.gold.ToString();
    }
}
