using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RelicVendor : MonoBehaviour
{
    void OnEnable()
    {
        NPCButton.OnButtonPress += HandleButtonPress;
    }

    void OnDisable()
    {
        NPCButton.OnButtonPress -= HandleButtonPress;
    }

    private void HandleButtonPress(NPCButton button)
    {
        ShowUI();
    }

    private void ShowUI()
    {
        Debug.Log("am rpessed");
    }
}
