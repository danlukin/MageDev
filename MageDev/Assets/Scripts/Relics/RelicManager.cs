using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using RelicGroup = System.Linq.IGrouping<string, RelicData>;

public class RelicManager : MonoBehaviour
{
    public static List<RelicData> relicList;
    public static IEnumerable<RelicGroup> groupedRelics;

    private PlayerSpellStats basic;
    private PlayerSpellStats super;
    private StatusEffect status;

    void OnEnable()
    {
        RelicNode.OnRelicUpdate += HandleRelicUpdate;
        relicList ??= new List<RelicData> ();

        basic = PlayerSpellManager.basicSpell;
        super = PlayerSpellManager.superSpell;
        status = PlayerSpellManager.status;
    }

    void OnDisable()
    {
        RelicNode.OnRelicUpdate -= HandleRelicUpdate;
    }

    public static RelicGroup FindRelicGroup(string relic)
    {
        if (groupedRelics != null)
        {
            foreach (RelicGroup group in groupedRelics)
            {
                if (group.Key == relic) return group;
            }
        }

        return null;
    }

    public static bool CheckLimit(RelicData relic)
    {
        RelicGroup group = FindRelicGroup(relic.relicName);
        if (group != null)
        {
            if (group.Count() < relic.stackLimit) return true;
            else return false;
        }
        else return true;
    }

    private void UpdateRelicList(RelicData relic, bool addRelic)
    {
        if (addRelic)
        {
            relicList.Add(relic);
            groupedRelics = relicList.GroupBy(relic => relic.relicName);
        }
        else
        {
            relicList.Remove(relic);
            groupedRelics = relicList.GroupBy(relic => relic.relicName);
        }
    }

    private bool HandleRelicList(RelicData relic, bool relicAdded)
    {
        if (relicAdded)
        {
            if (relicList.Count() == 0)
            {
                UpdateRelicList(relic, true);
                return true;
            }
            else if (relicList.Find(x => relic) == null)
            {
                UpdateRelicList(relic, true);
                return true;
            }
            else
            {
                if (CheckLimit(relic))
                {
                    UpdateRelicList(relic, true);
                    return true;
                }
            }
        }
        else
        {
            if (relicList.Count() > 0)
            {
                if (relicList.Find(x => relic) != null)
                {
                    UpdateRelicList(relic, false);
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleRelicUpdate(RelicData relic, bool relicAdded)
    {
        if (!HandleRelicList(relic, relicAdded)) return;

        switch (relic.relicName)
        {
            case "Basic Damage":
                if (relicAdded)
                {
                    basic.damageRelicMultiplier += 0.5f;
                }
                else
                {
                    basic.damageRelicMultiplier -= 0.5f;
                }
                basic.UpdateDamage();
                break;
            case "Super Damage":
                if (relicAdded)
                {
                    super.damageRelicMultiplier += 0.5f;
                }
                else
                {
                    super.damageRelicMultiplier += 0.5f;
                }
                super.UpdateDamage();
                break;
            case "Super Max Charge":
                if (relicAdded)
                {
                    super.maxChargeStacks += 15;
                }
                else
                {
                    super.maxChargeStacks -= 15;
                }
                break;
            case "Status Damage":
                if (relicAdded)
                {
                    status.damageRelicMultiplier += 0.5f;
                }
                else
                {
                    status.damageRelicMultiplier -= 0.5f;
                }
                status.UpdateDamage();
                break;
            case "Longer Status":
                if (relicAdded)
                {
                    status.durationRelicMultiplier += 1f;
                    status.baseDamage -= 1;
                }
                else
                {
                    status.durationRelicMultiplier -= 1f;
                    status.baseDamage += 1;
                }
                status.UpdateDuration();
                status.UpdateDamage();
                break;
        }


    }
}
