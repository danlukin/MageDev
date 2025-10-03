using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RelicInventoryNode : MonoBehaviour
{
    [SerializeField] public RelicData relicData;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Tooltip tooltip; 

    private int currentAmount;

    private void Awake()
    {
        if (relicData) UpdateUI();
        gameObject.name = relicData.relicName;
    }

    private void OnEnable()
    {
        RelicNodeManager.OnUpdateInventory += UpdateCurrentAmount;
        button.onClick.AddListener(() => ShowTooltip());
        FindTooltip();
    }

    private void OnDisable()
    {
        RelicNodeManager.OnUpdateInventory -= UpdateCurrentAmount;
        button.onClick.RemoveListener(() => ShowTooltip());
    }

    private void FindTooltip()
    {
        // Node -> RelicInventory -> Display -> Inventory
        tooltip = transform.parent.parent.parent.Find("InventoryTooltip").gameObject.GetComponentInChildren<Tooltip>(true);;
    }

    private void UpdateCurrentAmount(string relic, bool add)
    {
        if (relic == relicData.relicName)
        {
            if (add) ++currentAmount;
            else --currentAmount;

            if (currentAmount > 1) amountText.text = currentAmount.ToString();
            else amountText.text = "";
        }
    }

    private void UpdateUI()
    {
        icon.sprite = relicData.relicSprite;
    }

    public void ShowTooltip()
    {
        tooltip.ShowTooltip(relicData.relicName, relicData.Description);
    }
}
