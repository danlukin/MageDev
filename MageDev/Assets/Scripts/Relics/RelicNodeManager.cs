using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RelicNodeManager : MonoBehaviour
{
    [SerializeField] private GameObject relicObject;
    [SerializeField] private GameObject relicSelectUI;
    [SerializeField] private GameObject relicShopUI;
    [SerializeField] private GameObject relicInventory;

    private GameObject currentRelicUI;
    private GridLayoutGroup relicPanel;
    private RelicNode relicNode;
    private int choiceAmount = 3;
    private RelicNode currentActiveNode;

    private RelicData[] allRelics;

    public static event Action<string, bool> OnUpdateInventory;

    void Awake()
    {
        allRelics = Resources.LoadAll<RelicData>("RelicSO/");
        relicNode = relicObject.GetComponent<RelicNode>();
    }

    private void OnEnable()
    {
        RelicSelect.OnSelectActive += HandleSelectActive;
        RelicVendor.OnShopRefresh += InstantiateRelicPickUI;
    }

    private void OnDisable()
    {
        RelicSelect.OnSelectActive -= HandleSelectActive;
        RelicVendor.OnShopRefresh -= InstantiateRelicPickUI;
    }

    private void HandleSelectActive(string ui)
    {
        switch (ui)
        {
            case "RelicSelectUI":
                currentRelicUI = relicSelectUI;
                break;
            case "RelicShopUI":
                currentRelicUI = relicShopUI;
                break;
        }

        relicPanel = currentRelicUI.GetComponentInChildren<GridLayoutGroup>();
        InstantiateRelicPickUI();
    }

    private void InstantiateRelicPickUI()
    {
        List<int> selectedIndexes = new ();
        while (selectedIndexes.Count < 3)
        {
            int i = UnityEngine.Random.Range(0, allRelics.Length);
            if (!selectedIndexes.Contains(i) & RelicManager.CheckLimit(allRelics[i])) selectedIndexes.Add(i);
        }

        if (relicPanel.transform.childCount > 0) DestroyChildren();

        for (int i = 0; i < choiceAmount; ++i)
        {
            relicNode.relicData = allRelics[selectedIndexes[i]];
            RelicNode relicOption = Instantiate(relicNode);
            relicOption.transform.SetParent(relicPanel.transform, false);
            relicOption.relicButton.onClick.AddListener(() => HandleRelicSelect(relicOption));
        }
    }

    private void DestroyChildren()
    {
        for (int i = 0; i < relicPanel.transform.childCount; ++i)
        {
            Destroy(relicPanel.transform.GetChild(i).gameObject);
        }
    }

    private bool RelicCanBeAdded(RelicNode node)
    {
        if (currentRelicUI.name == "RelicShopUI" & node.relicCost > PlayerCurrency.gold) return false;
        return true;
    }

    private void HandleRelicSelect(RelicNode node)
    {
        if (node == currentActiveNode & RelicCanBeAdded(node))
        {
            node.AddRelic();
            AddRelicToInventory(node);
            DestroyChildren();

            switch (currentRelicUI.name)
            {
                case "RelicSelectUI":
                    currentRelicUI.SetActive(false);
                    break;
                case "RelicShopUI":
                    currentRelicUI.GetComponent<CanvasGroup>().alpha = 0;
                    currentRelicUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    PlayerCurrency.HandleCurrencyChange(Currency.Gold, -node.relicCost);
                    break;
            }
        }
        else
        {
            ShowTooltip(node);
            currentActiveNode = node;
        }

    }

    private void AddRelicToInventory(RelicNode node)
    {
        if (relicInventory.transform.Find(node.relicData.relicName) == null)
        {
            GameObject child = node.transform.Find("RelicInventoryNode").gameObject;
            child.transform.SetParent(relicInventory.transform, false);
            child.SetActive(true);
        }

        OnUpdateInventory?.Invoke(node.relicData.relicName, true);
    }

    private void ShowTooltip(RelicNode node)
    {
        currentRelicUI.GetComponentInChildren<Tooltip>(true).ShowTooltip(node.relicData.relicName, node.relicData.Description);
    }

}
