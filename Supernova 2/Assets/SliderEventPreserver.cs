// Script: SliderEventPreserver.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEventPreserver : MonoBehaviour, IPointerUpHandler
{
    private Slider slider;
    private BrightnessSlider brightnessSlider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        brightnessSlider = GetComponent<BrightnessSlider>();

        if (brightnessSlider == null)
        {
            brightnessSlider = GetComponentInParent<BrightnessSlider>();
        }
    }

    // Captura quando o usuário solta o slider
    public void OnPointerUp(PointerEventData eventData)
    {
        if (slider != null && brightnessSlider != null)
        {
            brightnessSlider.OnBrightnessChanged(slider.value);
        }
    }

    // Atualiza em tempo real enquanto arrasta
    public void OnDrag(PointerEventData eventData)
    {
        if (slider != null && brightnessSlider != null)
        {
            brightnessSlider.OnBrightnessChanged(slider.value);
        }
    }
}