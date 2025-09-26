using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    private GameObject StageUIManager;
    private CanvasGroup[] components;
    private CanvasGroup StageCanvas;
    private CanvasGroup TalentCanvas;
    private Button button;

    private void Start()
    {
        Setup();
    }

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void Setup()
    {
        StageUIManager = GameObject.Find("StageUIManager");
        components = StageUIManager.GetComponentsInChildren<CanvasGroup>(true);
        foreach (CanvasGroup x in components)
        {
            switch (x.name)
            {
                case "StageCanvas":
                    StageCanvas = x;
                    break;
                case "TalentCanvas":
                    TalentCanvas = x;
                    break;
            }
        }
    }

    private void Toggle()
    {
        if (StageCanvas.alpha == 1)
        {
            StageCanvas.alpha = 0;
            StageCanvas.blocksRaycasts = false;

            TalentCanvas.alpha = 1;
            TalentCanvas.blocksRaycasts = true;

            Time.timeScale = 0;
        }
        else
        {
            StageCanvas.alpha = 1;
            StageCanvas.blocksRaycasts = true;

            TalentCanvas.alpha = 0;
            TalentCanvas.blocksRaycasts = false;

            Time.timeScale = 1;
        }
    }
}

