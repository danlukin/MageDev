using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text contentText;

    public int characterWrapLimit;
    private int headerLength;
    private int contentLength;
    private LayoutElement layoutElement;

    static public UnityEvent anythingPressed = new();

    private void OnEnable()
    {
        gameObject.GetComponent<Canvas>().overrideSorting = true;
        anythingPressed.AddListener(HideTooltip);

        headerLength = headerText.text.Length;
        contentLength = contentText.text.Length;
        layoutElement = gameObject.GetComponent<LayoutElement>();
    }

    private void OnDisable()
    {
        anythingPressed.RemoveListener(HideTooltip);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            anythingPressed.Invoke();
        }
    }

    private void SetText(string header, string content)
    {
        headerText.text = header;
        contentText.text = content;

        headerLength = headerText.text.Length;
        contentLength = contentText.text.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }

    public void ShowTooltip(string header, string content)
    {
        gameObject.SetActive(true);
        SetText(header, content);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

}
