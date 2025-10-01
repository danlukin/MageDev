using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TalentNodeManager : MonoBehaviour
{
    [SerializeField] private Button respecButton;

    public TalentNode[] talentNodes;
    public TMP_Text talentPointsText;
    public static int availableTalentPoints;

    private TalentNode currentActiveNode;

    private void OnEnable()
    {
        TalentNode.OnTalentPointInteract += HandleTalentPointInteract;
        TalentNode.OnUnlockNext += HandleUnlockNext;
        PlayerExperience.OnLevelUp += HandleLevelUp;

        respecButton.onClick.AddListener(() => RespecAllNodes());
    }

    private void OnDisable()
    {
        TalentNode.OnTalentPointInteract -= HandleTalentPointInteract;
        TalentNode.OnUnlockNext -= HandleUnlockNext;
        PlayerExperience.OnLevelUp -= HandleLevelUp;

        foreach (TalentNode node in talentNodes)
        {
            node.talentButton.onClick.RemoveListener(() => CheckAvailablePoints(node));
        }

        respecButton.onClick.RemoveListener(() => RespecAllNodes());
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
        if (availableTalentPoints > 0 & currentActiveNode == node)
        {
            node.HandleUpgrade();
            node.ToggleTooltipActive();
        }
        else
        {
            node.ShowTooltip();
            currentActiveNode = node;
        }
    }

    public void UpdateTalentPoints(int amount)
    {
        availableTalentPoints += amount;
        talentPointsText.text = availableTalentPoints.ToString();
    }

    private void HandleTalentPointInteract(TalentNode node, bool upgrade)
    {
        if (upgrade)
        {
            if (availableTalentPoints > 0) UpdateTalentPoints(-1);
        }
        else
        {
            UpdateTalentPoints(1);
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

    private void RespecAllNodes()
    {
        foreach (TalentNode node in talentNodes)
        {
            if (node.currentRank > 0)
            {
                int rank = node.currentRank;
                for (int i = 0; i < rank; ++i)
                {
                    node.HandleDowngrade();
                }
            }
        }
    }
}
