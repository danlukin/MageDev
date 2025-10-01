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
    private IEnumerable<RelicGroup> groupedRelics;

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

    private RelicGroup FindRelicGroup(string relic)
    {
        foreach (RelicGroup group in groupedRelics)
        {
            if (group.Key == relic) return group;
        }

        return null;
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
                if (FindRelicGroup(relic.relicName).Count() < relic.stackLimit)
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
                    basic.baseDamage += 5;
                }
                else
                {
                    basic.baseDamage -= 5;
                }
                basic.UpdateDamage();
                break;
            case "Super Damage":
                if (relicAdded)
                {
                    super.baseDamage += 3;
                }
                else
                {
                    super.baseDamage -= 3;
                }
                super.UpdateDamage();
                break;
            case "Status Damage":
                if (relicAdded)
                {
                    status.DOTAmount += 2;
                }
                else
                {
                    status.DOTAmount -= 2;
                }
                break;
        }


    }
}
