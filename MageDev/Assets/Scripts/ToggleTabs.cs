using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTabs : MonoBehaviour
{
    [SerializeField] private TMP_Text tabName;

    [SerializeField] private GameObject relicTab;
    [SerializeField] private CanvasGroup relicCanvas;

    [SerializeField] private GameObject currencyTab;
    [SerializeField] private CanvasGroup currencyCanvas;

    private void OnEnable()
    {
        relicTab.GetComponent<Button>().onClick.AddListener(() => SetActiveTab("relic"));
        currencyTab.GetComponent<Button>().onClick.AddListener(() => SetActiveTab("currency"));
    }

    private void OnDisable()
    {
        relicTab.GetComponent<Button>().onClick.RemoveListener(() => SetActiveTab("relic"));
        currencyTab.GetComponent<Button>().onClick.RemoveListener(() => SetActiveTab("currency"));
        
    }

    private void SetActiveTab(string tab)
    {
        switch (tab)
        {
            case "relic":
                tabName.text = "Relics";

                relicCanvas.alpha = 1;
                relicCanvas.blocksRaycasts = true;

                currencyCanvas.alpha = 0;
                currencyCanvas.blocksRaycasts = false;
                break;
            case "currency":
                tabName.text = "Currency";

                relicCanvas.alpha = 0;
                relicCanvas.blocksRaycasts = false;

                currencyCanvas.alpha = 1;
                currencyCanvas.blocksRaycasts = true;
                break;
        }
    }
}
