using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Volume Keys")]
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    [Header("Default Values")]
    public float defaultMasterVolume = 0.5f;
    public float defaultMusicVolume = 0.5f;
    public float defaultSFXVolume = 0.5f;

    private void Awake()
    {
        // Implementação do Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        // Carrega os volumes salvos ou usa os padrões
        LoadAudioSettings();
    }

    public void SetMasterVolume(float volume)
    {
        // Converte o valor linear (0-1) para logarítmico (-80 a 0 dB)
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f) // Praticamente mudo
            volumeDB = -80f;

        audioMixer.SetFloat("MasterVolume", volumeDB);
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            volumeDB = -80f;

        audioMixer.SetFloat("MusicVolume", volumeDB);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            volumeDB = -80f;

        audioMixer.SetFloat("SFXVolume", volumeDB);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, defaultMasterVolume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
    }

    private void LoadAudioSettings()
    {
        // Aplica os volumes salvos ao mixer
        SetMasterVolume(GetMasterVolume());
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }

    // Método para resetar para os valores padrão
    public void ResetAudioSettings()
    {
        SetMasterVolume(defaultMasterVolume);
        SetMusicVolume(defaultMusicVolume);
        SetSFXVolume(defaultSFXVolume);
    }
}