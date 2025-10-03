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
    [SerializeField] private GameObject relicInventory;

    private GridLayoutGroup relicPanel;
    private RelicNode relicNode;
    private int choiceAmount = 3;
    private RelicNode currentActiveNode;

    private RelicData[] allRelics;

    public static event Action<string, bool> OnUpdateInventory;

    void Awake()
    {
        allRelics = Resources.LoadAll<RelicData>("RelicSO/");
        relicPanel = relicSelectUI.GetComponentInChildren<GridLayoutGroup>();
        relicNode = relicObject.GetComponent<RelicNode>();
    }

    private void OnEnable()
    {
        RelicSelect.OnSelectActive += HandleSelectActive;
    }

    private void OnDisable()
    {
        RelicSelect.OnSelectActive -= HandleSelectActive;
    }

    private void HandleSelectActive(RelicSelect obj)
    {
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

    private void HandleRelicSelect(RelicNode node)
    {
        if (node == currentActiveNode)
        {
            node.AddRelic();
            AddRelicToInventory(node);
            DestroyChildren();
            relicSelectUI.SetActive(false);
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
        relicSelectUI.GetComponentInChildren<Tooltip>(true).ShowTooltip(node.relicData.relicName, node.relicData.Description);
    }

}
