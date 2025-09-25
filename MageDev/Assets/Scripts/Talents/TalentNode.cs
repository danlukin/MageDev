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
    public bool isUnlocked;

    public Image talentIcon;
    public TMP_Text talentRankText;
    public Button talentButton;

    public static event Action<TalentNode> OnTalentPointSpent;
    public static event Action<TalentNode> OnNodeAllocated;

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
            OnNodeAllocated?.Invoke(this);
            UpdateUI();
        }
    }

    public void UnlockTalent()
    {
        isUnlocked = true;
        UpdateUI();
    }

    public bool CanUnlockTalent()
    {
        foreach (TalentNode node in prerequisites)
        {
            if (node.isUnlocked) return false;
        }

        Debug.Log(prerequisites);
        return true;
    }

}
