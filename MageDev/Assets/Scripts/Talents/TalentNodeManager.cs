using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentNodeManager : MonoBehaviour
{
    public TalentNode[] talentNodes;
    public TMP_Text talentPointsText;
    public int availableTalentPoints;

    private void OnEnable()
    {
        TalentNode.OnTalentPointSpent += HandleTalentPointSpent;
        TalentNode.OnUnlockNext += HandleUnlockNext;
        PlayerExperience.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        TalentNode.OnTalentPointSpent -= HandleTalentPointSpent;
        TalentNode.OnUnlockNext -= HandleUnlockNext;
        PlayerExperience.OnLevelUp -= HandleLevelUp;
    }

    void Start()
    {
        foreach (TalentNode node in talentNodes)
        {
            node.talentButton.onClick.AddListener(() => CheckAvailablePoints(node));
        }
        UpdateTalentPoints(0);
    }

    private void CheckAvailablePoints(TalentNode node)
    {
        if (availableTalentPoints > 0 & node.isActive)
        {
            node.HandleUpgrade();
            node.ToggleTooltipActive();
        }
        else
        {
            node.ShowTooltip();
        }
    }

    public void UpdateTalentPoints(int amount)
    {
        availableTalentPoints += amount;
        talentPointsText.text = availableTalentPoints.ToString();
    }

    private void HandleTalentPointSpent(TalentNode node)
    {
        if (availableTalentPoints > 0)
        {
            UpdateTalentPoints(-1);
        }
    }

    private void HandleUnlockNext(TalentNode _node)
    {
        foreach (TalentNode node in talentNodes)
        {
            if (!node.isUnlocked && node.CanUnlockTalent())
            {
                node.UnlockTalent();
            }
        }
    }
    
    private void HandleLevelUp(PlayerExperience experience)
    {
        UpdateTalentPoints(1);
    }
}
