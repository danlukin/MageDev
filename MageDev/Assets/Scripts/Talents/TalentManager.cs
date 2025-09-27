using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    [SerializeField] private PlayerSpell basic;
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
                basic.castRange += 2;
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
