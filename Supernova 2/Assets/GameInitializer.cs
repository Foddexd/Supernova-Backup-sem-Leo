using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Managers Prefabs")]
    public GameObject audioManagerPrefab;
    public GameObject brightnessManagerPrefab;
    public GameObject sceneLoaderPrefab;
    public GameObject darkOverlayPrefab;

    private void Awake()
    {
        // Cria AudioManager se não existir
        if (AudioManager.instance == null && audioManagerPrefab != null)
        {
            Instantiate(audioManagerPrefab);
        }

        // Cria BrightnessManager se não existir
        if (BrightnessManager.instance == null && brightnessManagerPrefab != null)
        {
            Instantiate(brightnessManagerPrefab);
        }

        // Cria SceneLoader se não existir - AGORA FUNCIONA!
        if (SceneLoader.instance == null && sceneLoaderPrefab != null)
        {
            GameObject loader = Instantiate(sceneLoaderPrefab);
            SceneLoader sceneLoader = loader.GetComponent<SceneLoader>();
            if (sceneLoader != null && darkOverlayPrefab != null)
            {
                sceneLoader.darkOverlayPrefab = darkOverlayPrefab;
            }
        }

        // Cria overlay inicial se necessário
        CreateInitialOverlay();
    }

    private void CreateInitialOverlay()
    {
        // Verifica se já existe um overlay
        if (GameObject.Find("DarkOverlay") == null && darkOverlayPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject overlay = Instantiate(darkOverlayPrefab, canvas.transform);
                overlay.name = "DarkOverlay";

                RectTransform rt = overlay.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                overlay.transform.SetAsLastSibling();
            }
        }
    }
}