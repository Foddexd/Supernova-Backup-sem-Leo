using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SimpleAudioManager : MonoBehaviour
{
    public static SimpleAudioManager instance;

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private float currentMasterVolume = 0.5f;

    private List<AudioSource> allAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadVolume()
    {
        currentMasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.5f);
        ApplyVolumeToAllSources();
    }

    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, currentMasterVolume);
        PlayerPrefs.Save();

        ApplyVolumeToAllSources();
    }

    public float GetMasterVolume()
    {
        return currentMasterVolume;
    }

    private void ApplyVolumeToAllSources()
    {
        foreach (AudioSource source in allAudioSources)
        {
            if (source != null)
            {
                // Multiplica o volume original pelo master
                AudioSourceVolumeKeeper keeper = source.GetComponent<AudioSourceVolumeKeeper>();
                if (keeper == null)
                {
                    keeper = source.gameObject.AddComponent<AudioSourceVolumeKeeper>();
                    keeper.originalVolume = source.volume;
                }

                source.volume = keeper.originalVolume * currentMasterVolume;
            }
        }
    }

    public void CollectAllAudioSources()
    {
        allAudioSources.Clear();
        AudioSource[] sources = FindObjectsOfType<AudioSource>(true);
        allAudioSources.AddRange(sources);

        // Aplica volume atual
        ApplyVolumeToAllSources();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke(nameof(DelayedCollect), 0.1f);
    }

    private void DelayedCollect()
    {
        CollectAllAudioSources();
    }
}