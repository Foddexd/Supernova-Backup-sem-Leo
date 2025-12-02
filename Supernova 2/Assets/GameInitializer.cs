using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [Header("Managers Prefabs")]
    public GameObject audioManagerPrefab;
    public GameObject brightnessManagerPrefab;
    public GameObject darkOverlayPrefab;

    [Header("Settings")]
    public bool createManagersIfMissing = true;
    public bool createOverlayInAllScenes = true;

    private void Awake()
    {
        Debug.Log("=== GAME INITIALIZER STARTING ===");

        // Garante que os managers existam
        EnsureManagersExist();

        // Cria overlay se necessário
        if (createOverlayInAllScenes)
        {
            CreateDarkOverlayInScene();
        }
    }

    private void EnsureManagersExist()
    {
        // AudioManager
        if (AudioManager.instance == null)
        {
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
                Debug.Log("AudioManager criado.");
            }
            else
            {
                Debug.LogWarning("AudioManager prefab não atribuído.");
            }
        }

        // BrightnessManager
        if (BrightnessManager.instance == null)
        {
            if (brightnessManagerPrefab != null)
            {
                Instantiate(brightnessManagerPrefab);
                Debug.Log("BrightnessManager criado.");
            }
            else
            {
                // Cria um manualmente
                GameObject bm = new GameObject("BrightnessManager");
                bm.AddComponent<BrightnessManager>();
                DontDestroyOnLoad(bm);
                Debug.Log("BrightnessManager criado manualmente.");
            }
        }
    }

    private void CreateDarkOverlayInScene()
    {
        // Verifica se já existe
        if (GameObject.Find("DarkOverlay") != null)
        {
            Debug.Log("DarkOverlay já existe na cena.");
            return;
        }

        if (darkOverlayPrefab == null)
        {
            Debug.LogWarning("DarkOverlay prefab não atribuído.");
            return;
        }

        // Encontra o Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            GameObject overlay = Instantiate(darkOverlayPrefab, canvas.transform);
            overlay.name = "DarkOverlay";

            // Configuração do RectTransform
            RectTransform rt = overlay.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                overlay.transform.SetAsLastSibling();
            }

            Debug.Log("DarkOverlay criado na cena.");
        }
        else
        {
            Debug.LogWarning("Canvas não encontrado para criar DarkOverlay.");
        }
    }
}