using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    private PlayerSpellStats basic;
    [SerializeField] private PlayerProjectile super;
    [SerializeField] private StatusEffectData status;

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

        switch (talentName)
        {
            case "Basic Damage":
                basic.baseDamage += 1;
                basic.UpdateDamage();
                break;
            case "Basic Status Chance":
                break;
            case "Basic Range":
                break;
            case "Super Damage":
                break;
            case "Super Area":
                break;
            case "Super Max Charge":
                break;
            case "Status Damage":
                break;
            case "Status Duration":
                break;
            case "Status Self Immolation":
                break;
            default:
                Debug.LogWarning("Unknown talent: " + talentName);
                break;
        }
    }
}
