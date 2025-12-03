using UnityEngine;

public class AudioSourceVolumeKeeper : MonoBehaviour
{
    [HideInInspector]
    public float originalVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            originalVolume = audioSource.volume;
        }
    }

    // Método para atualizar o volume quando o slider muda
    public void UpdateVolume(float masterVolume, float categoryVolume = 1f)
    {
        if (audioSource != null)
        {
            audioSource.volume = originalVolume * masterVolume * categoryVolume;
        }
    }
}