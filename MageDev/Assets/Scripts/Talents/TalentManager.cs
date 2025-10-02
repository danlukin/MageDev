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
        TalentNode.OnTalentPointInteract += HandleTalentPointInteract;
        
        basic = PlayerSpellManager.basicSpell;
        super = PlayerSpellManager.superSpell;
        status = PlayerSpellManager.status;
    }

    private void OnDisable()
    {
        TalentNode.OnTalentPointInteract -= HandleTalentPointInteract;
    }

    private void HandleTalentPointInteract(TalentNode node, bool upgrade)
    {
        string talentName = node.talentData.talentName;

        switch (talentName)
        {
            case "Basic Damage":
                if (upgrade) basic.baseDamage += 1;  
                else basic.baseDamage -= 1;
                basic.UpdateDamage();
                break;
            case "Basic Status Chance":
                if (upgrade) basic.statusChance += 0.1f;
                else basic.statusChance -= 0.1f;
                break;
            case "Basic Range":
                if (upgrade) PlayerSpellManager.HandleCastRangeUpdate(basic, '*', 2);
                else PlayerSpellManager.HandleCastRangeUpdate(basic, '/', 2);
                break;
            case "Super Damage":
                if (upgrade) super.baseDamage += 1;
                else super.baseDamage -= 1;
                super.UpdateDamage();
                break;
            case "Super Speed":
                if (upgrade) super.projSpeed += 0.5f;
                else super.projSpeed -= 0.5f;
                break;
            case "Super Max Charge":
                if (upgrade) super.maxChargeStacks += 10;
                else super.maxChargeStacks -= 10;
                break;
            case "Status Damage":
                if (upgrade) status.baseDamage += 1;
                else status.baseDamage -= 1;
                status.UpdateDamage();
                break;
            case "Status Duration":
                if (upgrade) status.baseDuration += 1;
                else status.baseDuration -= 1;
                break;
            case "Status Faster Damage":
                if (upgrade)
                {
                    status.tickRate *= 0.66f;
                    status.durationTalentMultiplier -= 0.33f;
                }
                else
                {
                    status.tickRate /= 0.66f;
                    status.durationTalentMultiplier += 0.33f;
                }
                break;
            default:
                Debug.LogWarning("Unknown talent: " + talentName);
                break;
        }
    }
}
