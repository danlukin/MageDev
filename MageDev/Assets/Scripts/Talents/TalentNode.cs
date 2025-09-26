using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentNode : MonoBehaviour
{
    public TalentData talentData;
    public List<TalentNode> prerequisites;

    public int currentRank;
    public bool isUnlocked = false;

    public Image talentIcon;
    public TMP_Text talentRankText;
    public Button talentButton;

    public static event Action<TalentNode> OnTalentPointSpent;
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
            OnTalentPointSpent?.Invoke(this);
            if (currentRank == 1) OnUnlockNext?.Invoke(this);
            UpdateUI();
        }
    }

    public bool CanUnlockTalent()
    {
        int minUnlockRank = 1;
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
}
