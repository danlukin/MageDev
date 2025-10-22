using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{

    private Slider slider;
    private new Camera camera;
    private CanvasGroup visibility;

    private void Awake()
    {
        camera = Camera.main;
        slider = GetComponent<Slider>();
        visibility = GetComponent<CanvasGroup>();
        ToggleVisibility(false);
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null) 
        {
            slider.value = currentValue / maxValue;
            ToggleVisibility(slider.value < 1);
        }
    }

    private void ToggleVisibility(bool visible)
    {
        if (visible) {visibility.alpha = 1;}
        else {visibility.alpha = 0;}
    }

    private void FixedUpdate()
    {
        gameObject.transform.rotation = camera.transform.rotation;
    }
}
