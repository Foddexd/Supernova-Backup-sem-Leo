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

    [Header("Configuration")]
    public bool useEventTrigger = true;

    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;

            // Carrega valor salvo
            LoadSavedVolume();

            // Configura evento
            if (!useEventTrigger)
            {
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            }

            UpdateUI();
        }
    }

    public void LoadSavedVolume()
    {
        if (AudioManager.instance != null)
        {
            float savedVolume = 0.5f;

            switch (volumeType)
            {
                case VolumeType.Master:
                    savedVolume = AudioManager.instance.GetMasterVolume();
                    break;
                case VolumeType.Music:
                    savedVolume = AudioManager.instance.GetMusicVolume();
                    break;
                case VolumeType.SFX:
                    savedVolume = AudioManager.instance.GetSFXVolume();
                    break;
            }

            volumeSlider.value = savedVolume;
        }
    }

    // Método para ser chamado pelo Inspector
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

    // Versão sem parâmetro para o Inspector
    public void OnVolumeChanged()
    {
        if (volumeSlider != null)
        {
            OnVolumeChanged(volumeSlider.value);
        }
    }

    private void UpdateUI()
    {
        if (volumeValueText != null)
        {
            int percentage = Mathf.RoundToInt(volumeSlider.value * 100);
            volumeValueText.text = $"{percentage}%";
        }

        if (muteIcon != null)
            muteIcon.enabled = volumeSlider.value <= 0.01f;

        if (maxVolumeIcon != null)
            maxVolumeIcon.enabled = volumeSlider.value >= 0.99f;
    }

    public void RefreshSlider()
    {
        LoadSavedVolume();
        UpdateUI();
    }

    public void ToggleMute()
    {
        if (volumeSlider.value > 0.01f)
        {
            volumeSlider.value = 0f;
            OnVolumeChanged(0f);
        }
        else
        {
            volumeSlider.value = 0.5f;
            OnVolumeChanged(0.5f);
        }
    }
}