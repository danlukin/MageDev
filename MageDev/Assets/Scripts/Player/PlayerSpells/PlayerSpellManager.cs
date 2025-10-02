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
    public float projSpeed;
    public float castSpeed;
    public float castRange;
    public float statusChance;
    public float damageRelicMultiplier = 1;

    public int maxChargeStacks;
    public float chargeMultiplier = 1;

    public void UpdateDamage()
    {
        damage = baseDamage * damageRelicMultiplier * chargeMultiplier;
    }
}

public class StatusEffect
{
    public string name;

    public float damage;
    public float baseDamage;
    public float damageRelicMultiplier = 1;

    public float tickRate;

    public float duration;
    public float baseDuration;
    public float durationRelicMultiplier = 1;
    public float durationTalentMultiplier = 1;

    public GameObject animation;

    public void UpdateDamage()
    {
        damage = baseDamage * damageRelicMultiplier;
    }

    public void UpdateDuration()
    {
        duration = baseDuration * durationRelicMultiplier * durationTalentMultiplier;
    }
}

public class PlayerSpellManager : MonoBehaviour
{
    [SerializeField] private PlayerSpellData basicSpellData;
    [SerializeField] private PlayerSpellData superSpellData;
    [SerializeField] private StatusEffectData statusData;

    public static PlayerSpellStats basicSpell = new PlayerSpellStats { };
    public static PlayerSpellStats superSpell = new PlayerSpellStats { };
    public static StatusEffect status = new StatusEffect { };

    public static event Action<PlayerSpellStats> OnCastRangeUpdated;

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
        spell.maxChargeStacks = data.maxChargeStacks;
    }

    private void InitStatus(StatusEffect status, StatusEffectData data)
    {
        status.name = data.Name;
        status.damage = data.Damage;
        status.baseDamage = data.Damage;
        status.tickRate = data.tickRate;
        status.duration = data.Duration;
        status.baseDuration = data.Duration;
        status.animation = data.StatusAnimation;
    }

    public static void SetChargeMultiplier(PlayerSpellStats spell, float amount)
    {
        spell.chargeMultiplier = amount;
        spell.UpdateDamage();
    }

    public static void HandleCastRangeUpdate(PlayerSpellStats spell, char operation, float amount)
    {
        switch (operation)
        {
            case '*':
                spell.castRange *= amount;
                break;
            case '/':
                spell.castRange /= amount;
                break;
            case '+':
                spell.castRange += amount;
                break;
            case '-':
                spell.castRange -= amount;
                break;
        }
        
        OnCastRangeUpdated?.Invoke(spell);
    }
}
