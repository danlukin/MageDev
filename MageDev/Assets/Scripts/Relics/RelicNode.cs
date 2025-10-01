using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicNode : MonoBehaviour
{
    [SerializeField] public RelicData relicData;
    [SerializeField] private Image relicIcon;
    [SerializeField] public Button relicButton;

    public static event Action<RelicData, bool> OnRelicUpdate;

    private void Awake()
    {
        if (relicData) UpdateUI();
        gameObject.name = relicData.relicName;
    }

    private void OnDestroy()
    {
        relicButton.onClick.RemoveAllListeners();
    }

    private void UpdateUI()
    {
        relicIcon.sprite = relicData.relicSprite;
    }

    public void AddRelic()
    {
        OnRelicUpdate?.Invoke(relicData, true);
    }

    public void RemoveRelic()
    {
        if (RelicManager.relicList.Find(x => relicData) != null)
        {
            OnRelicUpdate?.Invoke(relicData, false);
        }
    }
}
