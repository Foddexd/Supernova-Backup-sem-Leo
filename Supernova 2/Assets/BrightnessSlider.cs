using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrightnessSlider : MonoBehaviour
{
    [Header("UI References")]
    public Slider brightnessSlider;
    public TextMeshProUGUI brightnessValueText;
    public Image brightnessIcon;

    [Header("Icon Sprites")]
    public Sprite darkIcon;
    public Sprite mediumIcon;
    public Sprite lightIcon;

    private void Start()
    {
        brightnessSlider.minValue = 0f;
        brightnessSlider.maxValue = 1f;
        brightnessSlider.wholeNumbers = false;

        LoadSavedBrightness();
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        UpdateUI();
    }

    public void LoadSavedBrightness()
    {
        if (BrightnessManager.instance != null)
        {
            brightnessSlider.value = BrightnessManager.instance.GetBrightness();
        }
        else
        {
            brightnessSlider.value = 0.5f;
        }
    }

    public void OnBrightnessChanged(float value)
    {
        if (BrightnessManager.instance != null)
        {
            BrightnessManager.instance.SetBrightness(value);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (brightnessValueText != null)
        {
            int percentage = Mathf.RoundToInt(brightnessSlider.value * 100);
            brightnessValueText.text = $"{percentage}%";
        }

        if (brightnessIcon != null && darkIcon != null && mediumIcon != null && lightIcon != null)
        {
            if (brightnessSlider.value <= 0.33f)
                brightnessIcon.sprite = darkIcon;
            else if (brightnessSlider.value <= 0.66f)
                brightnessIcon.sprite = mediumIcon;
            else
                brightnessIcon.sprite = lightIcon;
        }
    }

    // Adicione este método
    public void RefreshSlider()
    {
        LoadSavedBrightness();
        UpdateUI();
    }

    public void ResetToDefault()
    {
        brightnessSlider.value = 0.5f;
        UpdateUI();
    }
}