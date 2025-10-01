using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TalentNode : MonoBehaviour
{
    public TalentData talentData;
    public List<TalentNode> prerequisites;
    public TalentNode nextNode;

    public int currentRank;
    public bool isUnlocked = false;

    public Image talentIcon;
    public TMP_Text talentRankText;
    public Button talentButton;

    public bool tooltipIsActive = false;

    private int minUnlockRank = 1;

    public static event Action<TalentNode, bool> OnTalentPointInteract;
    public static event Action<TalentNode> OnUnlockNext;

    private void OnValidate()
    {
        if (talentData) UpdateUI();
    }

    private void UpdateUI()
    {
        talentIcon.sprite = talentData.talentIcon;

        if (isUnlocked)
        {
            talentButton.interactable = true;
            talentRankText.text = currentRank.ToString() + "/" + talentData.maxRank;
            talentIcon.color = Color.white;
        }
        else
        {
            talentButton.interactable = false;
            talentRankText.text = "";
            talentIcon.color = Color.gray;
        }
    }

    public void HandleUpgrade()
    {
        if (isUnlocked && currentRank < talentData.maxRank)
        {
            ++currentRank;
            OnTalentPointInteract?.Invoke(this, true);
            if (currentRank == 1) OnUnlockNext?.Invoke(this);
            UpdateUI();
        }
    }

    public void HandleDowngrade()
    {
        if (currentRank >= 1)
        {
            --currentRank;
            OnTalentPointInteract?.Invoke(this, false);
            if (currentRank < minUnlockRank & nextNode != null)
            {
                nextNode.isUnlocked = false;
                nextNode.UpdateUI();
            }
            UpdateUI();
        }
    }

    public bool CanUnlockTalent()
    {
        foreach (TalentNode node in prerequisites)
        {
            if (!node.isUnlocked || node.currentRank < minUnlockRank) return false;
        }

        return true;
    }

    public void UnlockTalent()
    {
        isUnlocked = true;
        UpdateUI();
    }

    public void ShowTooltip()
    {
        talentButton.GetComponentInChildren<Tooltip>(true).ShowTooltip(talentData.talentName, talentData.talentTooltip);
        ToggleTooltipActive();
    }

    public void ToggleTooltipActive()
    {
        tooltipIsActive = !tooltipIsActive;
    }
}
