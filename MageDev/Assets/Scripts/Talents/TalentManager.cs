using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    private PlayerSpellStats basic;
    private PlayerSpellStats super;
    private StatusEffect status;

    private void OnEnable()
    {
        TalentNode.OnTalentPointSpent += HandleTalentPointSpent;
    }

    void OnDisable()
    {
        TalentNode.OnTalentPointSpent -= HandleTalentPointSpent;
    }

    private void HandleTalentPointSpent(TalentNode node)
    {
        string talentName = node.talentData.talentName;
        basic = PlayerSpellManager.basicSpell;
        super = PlayerSpellManager.superSpell;
        status = PlayerSpellManager.status;

        switch (talentName)
        {
            case "Basic Damage":
                basic.baseDamage += 1;
                basic.UpdateDamage();
                break;
            case "Basic Status Chance":
                basic.statusChance += 0.1f;
                break;
            case "Basic Range":
                PlayerSpellManager.HandleCastRangeUpdate(basic, '*', 2);
                break;
            case "Super Damage":
                super.baseDamage += 1;
                super.UpdateDamage();
                break;
            case "Super Speed":
                super.projSpeed += 0.5f;
                break;
            case "Super Max Charge":
                super.maxChargeStacks += 10;
                break;
            case "Status Damage":
                status.DOTAmount += 1;
                break;
            case "Status Duration":
                status.duration += 1;
                break;
            case "Status Faster Damage":
                status.tickRate *= 0.66f;
                status.duration *= 0.66f;
                break;
            default:
                Debug.LogWarning("Unknown talent: " + talentName);
                break;
        }
    }
}
