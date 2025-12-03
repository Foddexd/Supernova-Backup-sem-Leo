using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Mixer Groups")]
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    [Header("Volume Keys")]
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    [Header("Default Values")]
    public float defaultMasterVolume = 0.5f;
    public float defaultMusicVolume = 0.5f;
    public float defaultSFXVolume = 0.5f;

    [Header("Audio Source Tracking")]
    private List<AudioSource> allAudioSources = new List<AudioSource>();
    private List<AudioSource> musicAudioSources = new List<AudioSource>();
    private List<AudioSource> sfxAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void InitializeAudio()
    {
        LoadAudioSettings();
    }

    public void SetMasterVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            volumeDB = -80f;

        // Aplica no mixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", volumeDB);
        }

        // Aplica manualmente em todos os AudioSources
        ApplyMasterVolumeToSources(volume);

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        Debug.Log($"Master Volume ajustado para: {volume} ({volumeDB}dB)");
    }

    public void SetMusicVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            volumeDB = -80f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", volumeDB);
        }

        ApplyMusicVolumeToSources(volume);

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20;
        if (volume <= 0.0001f)
            volumeDB = -80f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", volumeDB);
        }

        ApplySFXVolumeToSources(volume);

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

    // Método para recolher todos os AudioSources da cena
    public void CollectAllAudioSources()
    {
        allAudioSources.Clear();
        musicAudioSources.Clear();
        sfxAudioSources.Clear();

        // Encontra TODOS os AudioSources na cena (incluindo inativos)
        AudioSource[] sources = FindObjectsOfType<AudioSource>(true);

        foreach (AudioSource source in sources)
        {
            if (source == null) continue;

            allAudioSources.Add(source);

            // Classifica baseado em tags, nomes ou outros critérios
            if (IsMusicSource(source))
            {
                musicAudioSources.Add(source);
                // Conecta ao grupo de música se disponível
                if (musicGroup != null)
                {
                    source.outputAudioMixerGroup = musicGroup;
                }
            }
            else
            {
                sfxAudioSources.Add(source);
                // Conecta ao grupo de SFX se disponível
                if (sfxGroup != null)
                {
                    source.outputAudioMixerGroup = sfxGroup;
                }
            }
        }

        Debug.Log($"Coletados {allAudioSources.Count} AudioSources " +
                  $"(Música: {musicAudioSources.Count}, SFX: {sfxAudioSources.Count})");
    }

    private bool IsMusicSource(AudioSource source)
    {
        // Lógica para identificar música:
        // 1. Por tag
        if (source.CompareTag("Music") || source.CompareTag("BackgroundMusic"))
            return true;

        // 2. Por nome do GameObject
        string name = source.gameObject.name.ToLower();
        if (name.Contains("music") || name.Contains("bgm") || name.Contains("background"))
            return true;

        // 3. Por nome do AudioClip
        if (source.clip != null)
        {
            string clipName = source.clip.name.ToLower();
            if (clipName.Contains("music") || clipName.Contains("theme") ||
                clipName.Contains("background") || clipName.Contains("ost"))
                return true;
        }

        return false;
    }

    // Aplica volume master a todos os sources
    private void ApplyMasterVolumeToSources(float masterVolume)
    {
        foreach (AudioSource source in allAudioSources)
        {
            if (source != null)
            {
                // Se o source não tem AudioSourceVolumeKeeper, adiciona
                AudioSourceVolumeKeeper keeper = source.GetComponent<AudioSourceVolumeKeeper>();
                if (keeper == null)
                {
                    keeper = source.gameObject.AddComponent<AudioSourceVolumeKeeper>();
                    keeper.originalVolume = source.volume;
                }

                // Calcula volume baseado no tipo
                float categoryVolume = 1f;
                if (musicAudioSources.Contains(source))
                    categoryVolume = GetMusicVolume();
                else if (sfxAudioSources.Contains(source))
                    categoryVolume = GetSFXVolume();

                // Aplica volume
                source.volume = keeper.originalVolume * masterVolume * categoryVolume;
            }
        }
    }

    private void ApplyMusicVolumeToSources(float musicVolume)
    {
        foreach (AudioSource source in musicAudioSources)
        {
            if (source != null)
            {
                AudioSourceVolumeKeeper keeper = source.GetComponent<AudioSourceVolumeKeeper>();
                if (keeper == null)
                {
                    keeper = source.gameObject.AddComponent<AudioSourceVolumeKeeper>();
                    keeper.originalVolume = source.volume;
                }

                source.volume = keeper.originalVolume * GetMasterVolume() * musicVolume;
            }
        }
    }

    private void ApplySFXVolumeToSources(float sfxVolume)
    {
        foreach (AudioSource source in sfxAudioSources)
        {
            if (source != null)
            {
                AudioSourceVolumeKeeper keeper = source.GetComponent<AudioSourceVolumeKeeper>();
                if (keeper == null)
                {
                    keeper = source.gameObject.AddComponent<AudioSourceVolumeKeeper>();
                    keeper.originalVolume = source.volume;
                }

                source.volume = keeper.originalVolume * GetMasterVolume() * sfxVolume;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Aguarda um frame para garantir que todos os objetos foram carregados
        Invoke(nameof(DelayedAudioSetup), 0.1f);
    }

    private void DelayedAudioSetup()
    {
        CollectAllAudioSources();
        LoadAudioSettings();
    }

    // Método para forçar atualização imediata
    public void ForceUpdateAllAudioSources()
    {
        CollectAllAudioSources();
        LoadAudioSettings();
    }
}