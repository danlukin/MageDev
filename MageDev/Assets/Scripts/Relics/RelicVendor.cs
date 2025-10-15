using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RelicVendor : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button closeButton;

    private int refreshCost = 1;
    private CanvasGroup shopCanvas;

    public static event Action OnShopRefresh;

    void Awake()
    {
        shopCanvas = shop.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        NPCButton.OnButtonPress += HandleButtonPress;

        closeButton.onClick.AddListener(() => CloseShop());
        refreshButton.onClick.AddListener(() => RefreshShop());
    }

    void OnDisable()
    {
        NPCButton.OnButtonPress -= HandleButtonPress;

        closeButton.onClick.RemoveListener(() => CloseShop());
        refreshButton.onClick.RemoveListener(() => RefreshShop());
    }

    private void HandleButtonPress(NPCButton button)
    {
        OpenShop();
    }

    private void OpenShop()
    {
        shopCanvas.alpha = 1;
        shopCanvas.blocksRaycasts = true;
    }

    private void CloseShop()
    {
        shopCanvas.alpha = 0;
        shopCanvas.blocksRaycasts = false;
    }

    private void RefreshShop()
    {
        if (PlayerCurrency.gold >= refreshCost)
        {
            PlayerCurrency.HandleCurrencyChange(Currency.Gold, -refreshCost);
            OnShopRefresh?.Invoke();
        }
    }
}
