using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRelic", menuName = "Relic")]
public class RelicData : ScriptableObject
{
    public string relicName;
    public Sprite relicSprite;
    public int stackLimit = 1;
    public string Description;
}
