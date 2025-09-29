using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Basic,
    Super
}

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell")]
public class PlayerSpellData : ScriptableObject
{
    [Header("Generic Values")]
    public string Name;
    public Sprite Sprite;
    public SpellType spellType;
    public float Damage;
    public float projSpeed;
    public float castSpeed;
    public float castRange;
    public float statusChance;
    
    [Header("Super Values")]
    public int maxChargeStacks = 1;
}
