using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellStats
{
    public SpellType spellType;
    public Sprite sprite;
    public string name;
    public float damage;
    public float baseDamage;
    public float damageMultiplier = 1;
    public float chargeMultiplier = 1;
    public float projSpeed;
    public float castSpeed;
    public float castRange;
    public float statusChance;

    public void UpdateDamage()
    {
        damage = baseDamage * damageMultiplier * chargeMultiplier;
    }
}

public class StatusEffect
{
    public string name;
    public float DOTAmount;
    public float HOTAmount;
    public float tickRate;
    public float duration;
}

public class PlayerSpellManager : MonoBehaviour
{
    [SerializeField] private PlayerSpellData basicSpellData;
    [SerializeField] private PlayerSpellData superSpellData;
    [SerializeField] private StatusEffectData statusData;

    public static PlayerSpellStats basicSpell = new PlayerSpellStats { };
    public static PlayerSpellStats superSpell = new PlayerSpellStats { };
    public static StatusEffect status = new StatusEffect { };

    private void Awake()
    {
        InitSpell(basicSpell, basicSpellData);
        InitSpell(superSpell, superSpellData);
        InitStatus(status, statusData);
    }

    private void InitSpell(PlayerSpellStats spell, PlayerSpellData data)
    {
        spell.spellType = data.spellType;
        spell.sprite = data.Sprite;
        spell.name = data.Name;
        spell.damage = data.Damage;
        spell.baseDamage = data.Damage;
        spell.projSpeed = data.projSpeed;
        spell.castSpeed = data.castSpeed;
        spell.castRange = data.castRange;
        spell.statusChance = data.statusChance;
    }

    private void InitStatus(StatusEffect status, StatusEffectData data)
    {
        status.name = data.name;
        status.DOTAmount = data.DOTAmount;
        status.HOTAmount = data.HOTAmount;
        status.tickRate = data.tickRate;
        status.duration = data.Duration;
    }

    public static void SetChargeMultiplier(float amount)
    {
        superSpell.chargeMultiplier = amount;
        superSpell.UpdateDamage();
    }
}
