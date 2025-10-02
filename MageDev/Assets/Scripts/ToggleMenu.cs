using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainCanvas;
    [SerializeField] private CanvasGroup menuCanvas;

    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void Toggle()
    {
        if (mainCanvas.alpha == 1)
        {
            mainCanvas.alpha = 0;
            mainCanvas.blocksRaycasts = false;

            menuCanvas.alpha = 1;
            menuCanvas.blocksRaycasts = true;

            Time.timeScale = 0;
        }
        else
        {
            mainCanvas.alpha = 1;
            mainCanvas.blocksRaycasts = true;

            menuCanvas.alpha = 0;
            menuCanvas.blocksRaycasts = false;

            Time.timeScale = 1;
        }
    }
}

