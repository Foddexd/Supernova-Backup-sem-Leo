using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [Header("Settings")]
    public VolumeType volumeType = VolumeType.Master;

    [Header("UI References")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText;
    public Image muteIcon;
    public Image maxVolumeIcon;

    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    private void Start()
    {
        // Configura o slider
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.wholeNumbers = false;

        // Carrega o valor salvo
        LoadSavedVolume();

        // Adiciona o listener para mudanças no slider
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Atualiza a UI inicial
        UpdateUI();
    }

    private void LoadSavedVolume()
    {
        if (AudioManager.instance != null)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    volumeSlider.value = AudioManager.instance.GetMasterVolume();
                    break;
                case VolumeType.Music:
                    volumeSlider.value = AudioManager.instance.GetMusicVolume();
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = AudioManager.instance.GetSFXVolume();
                    break;
            }
        }
        else
        {
            // Fallback se o AudioManager não estiver disponível
            volumeSlider.value = 0.5f;
            Debug.LogWarning("AudioManager não encontrado! Usando valor padrão.");
        }
    }

    public void OnVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    AudioManager.instance.SetMasterVolume(value);
                    break;
                case VolumeType.Music:
                    AudioManager.instance.SetMusicVolume(value);
                    break;
                case VolumeType.SFX:
                    AudioManager.instance.SetSFXVolume(value);
                    break;
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Atualiza o texto de porcentagem
        if (volumeValueText != null)
        {
            int percentage = Mathf.RoundToInt(volumeSlider.value * 100);
            volumeValueText.text = $"{percentage}%";
        }

        // Atualiza ícones de mudo/volume máximo
        if (muteIcon != null)
            muteIcon.enabled = volumeSlider.value <= 0.01f;

        if (maxVolumeIcon != null)
            maxVolumeIcon.enabled = volumeSlider.value >= 0.99f;
    }

    // Método para mute rápido (opcional)
    public void ToggleMute()
    {
        if (volumeSlider.value > 0.01f)
        {
            volumeSlider.value = 0f;
        }
        else
        {
            volumeSlider.value = 0.5f; // Volta para o meio
        }
    }
}