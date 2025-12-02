using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager instance;

    [Header("UI References")]
    public Image darkOverlay;
    public CanvasGroup darkOverlayCanvasGroup;

    [Header("Settings")]
    private const string BRIGHTNESS_KEY = "BrightnessValue";
    private float brightnessValue = 0.5f;

    private const float MIN_ALPHA = 0f;
    private const float MAX_ALPHA = 200f / 255f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            LoadBrightness();
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

    public void InitializeBrightness()
    {
        LoadBrightness();
        ApplyBrightness();
    }

    public void SetBrightness(float value)
    {
        brightnessValue = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(BRIGHTNESS_KEY, brightnessValue);
        PlayerPrefs.Save();
        ApplyBrightness();
    }

    public float GetBrightness()
    {
        return brightnessValue;
    }

    private void LoadBrightness()
    {
        if (PlayerPrefs.HasKey(BRIGHTNESS_KEY))
        {
            brightnessValue = PlayerPrefs.GetFloat(BRIGHTNESS_KEY);
        }
        else
        {
            brightnessValue = 0.5f;
        }
    }

    public void ApplyBrightness()
    {
        float alpha = Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, brightnessValue);

        // Tenta encontrar o overlay se não estiver atribuído
        if (darkOverlay == null && darkOverlayCanvasGroup == null)
        {
            FindDarkOverlay();
        }

        if (darkOverlay != null)
        {
            Color color = darkOverlay.color;
            color.a = alpha;
            darkOverlay.color = color;
        }

        if (darkOverlayCanvasGroup != null)
        {
            darkOverlayCanvasGroup.alpha = alpha;
        }
    }

    public void FindDarkOverlay()
    {
        GameObject overlayObj = GameObject.Find("DarkOverlay");
        if (overlayObj != null)
        {
            darkOverlay = overlayObj.GetComponent<Image>();
            if (darkOverlay == null)
            {
                darkOverlayCanvasGroup = overlayObj.GetComponent<CanvasGroup>();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Atraso para garantir que a cena foi totalmente carregada
        Invoke(nameof(DelayedBrightnessApply), 0.1f);
    }

    private void DelayedBrightnessApply()
    {
        FindDarkOverlay();
        ApplyBrightness();
    }

    public void ResetBrightness()
    {
        SetBrightness(0.5f);
    }
}