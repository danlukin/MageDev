using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public static event Action<PlayerExperience> OnLevelUp;

    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private EXPBar expBar;
    private float currentExp;
    private float expToLvlUp;
    private int currentLvl = 1;

    private void Awake()
    {
        SetExpToLvlUp();
        SetLevelText();
    }

    private void SetExpToLvlUp()
    {
        expToLvlUp = MathF.Pow(currentLvl, 3) + 2;
    }

    public void GrantExperience(float amount)
    {
        currentExp += amount;
        expBar.UpdateEXPBar(currentExp, expToLvlUp);
        CheckForLvlUp();
    }

    private void CheckForLvlUp()
    {
        if (currentExp >= expToLvlUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        ++currentLvl;
        currentExp = 0;
        SetExpToLvlUp();
        SetLevelText();
        expBar.UpdateEXPBar(currentExp, expToLvlUp);
        OnLevelUp?.Invoke(this);
    }

    private void SetLevelText()
    {
        lvlText.text = "Level: " + currentLvl;
    }
}
