using UnityEngine;
using UnityEngine.Audio;

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

    // Mantenha APENAS este método (remova o outro)
    public void InitializeAudio()
    {
        LoadAudioSettings();
    }

    public void SetMasterVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
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

    public void LoadAudioSettings()
    {
        SetMasterVolume(GetMasterVolume());
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }

    public void ResetAudioSettings()
    {
        SetMasterVolume(defaultMasterVolume);
        SetMusicVolume(defaultMusicVolume);
        SetSFXVolume(defaultSFXVolume);
    }

    public void OnSceneLoaded()
    {
        LoadAudioSettings();
    }
}