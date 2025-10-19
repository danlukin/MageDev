using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSelect : MonoBehaviour
{
    public static event Action<string> OnSelectActive;

    void OnEnable()
    {
        StartCoroutine(DelayedActivation());
    }

    void OnDisable()
    {
        //Time.timeScale = 1;
        GameManager.TogglePause();
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(0.1f);

        //Time.timeScale = 0;
        GameManager.TogglePause();
        OnSelectActive?.Invoke(gameObject.name);
    }

}
