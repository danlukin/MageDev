using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;

    public static event Action<NPCButton> OnButtonPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        OnButtonPress?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
