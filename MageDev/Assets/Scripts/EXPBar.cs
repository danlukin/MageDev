using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text currentEXPText;
    [SerializeField] private TMP_Text maxEXPText;

    public void UpdateEXPBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        currentEXPText.text = currentValue.ToString();
        maxEXPText.text = maxValue.ToString();
    }
}
