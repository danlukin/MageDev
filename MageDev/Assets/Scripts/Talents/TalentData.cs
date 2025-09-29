using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewTalent", menuName = "Talent Tree")]
public class TalentData : ScriptableObject
{
    public string talentName;
    public int maxRank;
    public Sprite talentIcon;
    public string talentTooltip;
}
