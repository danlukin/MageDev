using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject enemyManager;
    [SerializeField] private GameObject stageManager;

    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject camp;

    public static event Action<GameState> OnTravel;

    void OnEnable()
    {
        button.onClick.AddListener(() => Travel());
    }
        void OnDisable()
    {
        button.onClick.RemoveListener(() => Travel());
    }

    private void Travel()
    {
        switch (GameManager.State)
        {
            case GameState.Stage:
                enemyManager.SetActive(false);
                stageManager.SetActive(false);

                stage.SetActive(false);
                camp.SetActive(true);

                OnTravel?.Invoke(GameState.Camp);
                break;
            case GameState.Camp:
                enemyManager.SetActive(true);
                stageManager.SetActive(true);

                stage.SetActive(true);
                camp.SetActive(false);

                OnTravel?.Invoke(GameState.Stage);
                break;
        }
    }
}
