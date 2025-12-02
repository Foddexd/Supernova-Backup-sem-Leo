using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    [Header("Configuration")]
    public bool useEventTrigger = true; // Usar Event Trigger do Inspector

    private void Start()
    {
        // Configuração básica do slider
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.wholeNumbers = false;

            // SÓ adiciona via código se não configurado no Inspector
            if (!useEventTrigger)
            {
                brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
                Debug.Log("Listener adicionado via código");
            }

            // Inicializa o slider
            StartCoroutine(InitializeSliderDelayed());
        }
    }

    private IEnumerator InitializeSliderDelayed()
    {
        // Aguarda um frame para garantir que o BrightnessManager foi inicializado
        yield return null;

        LoadSavedBrightness();
        UpdateUI();

        Debug.Log($"Slider inicializado com valor: {brightnessSlider.value}");
    }

    public void LoadSavedBrightness()
    {
        if (BrightnessManager.instance != null)
        {
            brightnessSlider.value = BrightnessManager.instance.GetBrightness();
            Debug.Log($"Valor carregado: {brightnessSlider.value}");
        }
        else
        {
            brightnessSlider.value = 0.5f;
            Debug.LogWarning("BrightnessManager não encontrado! Usando valor padrão.");
        }
    }

    // MÉTODO PÚBLICO para ser chamado pelo Inspector
    public void OnBrightnessChanged(float value)
    {
        if (BrightnessManager.instance != null)
        {
            BrightnessManager.instance.SetBrightness(value);
        }
        else
        {
            Debug.LogError("BrightnessManager não encontrado!");
        }

        UpdateUI();
    }

    // Versão sem parâmetro para o Inspector
    public void OnBrightnessChanged()
    {
        if (brightnessSlider != null)
        {
            OnBrightnessChanged(brightnessSlider.value);
        }
    }

    private void UpdateUI()
    {
        // Atualiza o texto de porcentagem
        if (brightnessValueText != null)
        {
            int percentage = Mathf.RoundToInt(brightnessSlider.value * 100);
            brightnessValueText.text = $"{percentage}%";
        }

        // Atualiza o ícone
        if (brightnessIcon != null)
        {
            if (brightnessSlider.value <= 0.33f)
                brightnessIcon.sprite = darkIcon;
            else if (brightnessSlider.value <= 0.66f)
                brightnessIcon.sprite = mediumIcon;
            else
                brightnessIcon.sprite = lightIcon;
        }
    }

    public void RefreshSlider()
    {
        LoadSavedBrightness();
        UpdateUI();
    }

    public void ResetToDefault()
    {
        brightnessSlider.value = 0.5f;
        OnBrightnessChanged(0.5f);
    }
}