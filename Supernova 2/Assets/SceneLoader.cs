using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // Singleton instance - ADICIONADO
    public static SceneLoader instance;

    [Header("Configuração por Cena")]
    public bool desativarRedirecionamento = false;

    [Header("Sistema de Brilho")]
    public GameObject darkOverlayPrefab; // Prefab do overlay de brilho
    public bool criarOverlayBrilho = true;
    public string tagCanvasPrincipal = "MainCanvas";

    // Método Awake para singleton - ADICIONADO
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Não use DontDestroyOnLoad se você quer um SceneLoader por cena
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GAMEMANAGER NÃO ENCONTRADO!");
            return;
        }

        Debug.Log($"=== SCENELOADER - {SceneManager.GetActiveScene().name} ===");

        // Configura o sistema de brilho
        SetupBrightnessSystem();

        if (!desativarRedirecionamento)
        {
            CheckSceneRedirect();
        }
    }

    private void SetupBrightnessSystem()
    {
        // Garante que o BrightnessManager exista
        EnsureBrightnessManagerExists();

        // Cria o overlay de brilho se necessário
        if (criarOverlayBrilho && darkOverlayPrefab != null)
        {
            CreateDarkOverlay();
        }

        // Aplica as configurações de brilho salvas
        ApplySavedBrightness();

        // Aplica as configurações de áudio salvas
        ApplySavedAudio();
    }

    private void EnsureBrightnessManagerExists()
    {
        // Se não existe um BrightnessManager, tenta encontrar ou criar
        if (BrightnessManager.instance == null)
        {
            // Procura por um existente na cena
            BrightnessManager existingManager = FindObjectOfType<BrightnessManager>();
            if (existingManager == null)
            {
                // Cria um novo GameObject com BrightnessManager
                GameObject managerObj = new GameObject("BrightnessManager");
                managerObj.AddComponent<BrightnessManager>();
                DontDestroyOnLoad(managerObj);
                Debug.Log("BrightnessManager criado automaticamente.");
            }
        }
    }

    private void CreateDarkOverlay()
    {
        // Verifica se já existe um overlay na cena
        GameObject existingOverlay = GameObject.Find("DarkOverlay");
        if (existingOverlay != null)
        {
            Debug.Log("DarkOverlay já existe na cena.");

            // Atualiza a referência no BrightnessManager
            if (BrightnessManager.instance != null)
            {
                BrightnessManager.instance.darkOverlay = existingOverlay.GetComponent<Image>();
                if (BrightnessManager.instance.darkOverlay == null)
                {
                    BrightnessManager.instance.darkOverlayCanvasGroup = existingOverlay.GetComponent<CanvasGroup>();
                }
            }
            return;
        }

        // Encontra o Canvas principal
        GameObject canvasObj = GameObject.FindGameObjectWithTag(tagCanvasPrincipal);
        if (canvasObj == null)
        {
            // Tenta encontrar qualquer Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                canvasObj = canvas.gameObject;
            }
        }

        if (canvasObj != null)
        {
            // Instancia o overlay
            GameObject overlayObj = Instantiate(darkOverlayPrefab, canvasObj.transform);
            overlayObj.name = "DarkOverlay";

            // Configura para cobrir toda a tela
            RectTransform rectTransform = overlayObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }

            // Configura como último filho (para ficar no topo)
            overlayObj.transform.SetAsLastSibling();

            // Atualiza a referência no BrightnessManager
            if (BrightnessManager.instance != null)
            {
                BrightnessManager.instance.darkOverlay = overlayObj.GetComponent<Image>();
                if (BrightnessManager.instance.darkOverlay == null)
                {
                    BrightnessManager.instance.darkOverlayCanvasGroup = overlayObj.GetComponent<CanvasGroup>();
                }
            }

            Debug.Log("DarkOverlay criado com sucesso.");
        }
        else
        {
            Debug.LogWarning($"Canvas com tag '{tagCanvasPrincipal}' não encontrado na cena.");
        }
    }

    private void ApplySavedBrightness()
    {
        if (BrightnessManager.instance != null)
        {
            // Aguarda um frame para garantir que tudo está carregado
            StartCoroutine(ApplyBrightnessDelayed());
        }
    }

    private IEnumerator ApplyBrightnessDelayed()
    {
        yield return null; // Aguarda um frame

        BrightnessManager.instance.FindDarkOverlay();
        BrightnessManager.instance.ApplyBrightness();
        Debug.Log("Configurações de brilho aplicadas.");
    }

    private void ApplySavedAudio()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.LoadAudioSettings();
            Debug.Log("Configurações de áudio aplicadas.");
        }
    }

    private void CheckSceneRedirect()
    {
        int currentCheckpoint = GameManager.instance.GetCurrentCheckpoint();
        string currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log($"Verificando: Cena='{currentSceneName}', Checkpoint={currentCheckpoint}");

        // Redireciona apenas se necessário
        if (currentSceneName == GameManager.instance.cenaAto1 && currentCheckpoint >= 2)
        {
            Debug.Log($"Redirecionando: {currentSceneName} -> {GameManager.instance.cenaAto2e3}");
            SceneManager.LoadScene(GameManager.instance.cenaAto2e3);
        }
        else if (currentSceneName == GameManager.instance.cenaAto2e3 && currentCheckpoint >= 3)
        {
            Debug.Log($"Redirecionando: {currentSceneName} -> {GameManager.instance.cenaAto3}");
            SceneManager.LoadScene(GameManager.instance.cenaAto3);
        }
        else
        {
            Debug.Log("Nenhum redirecionamento necessário");
        }
    }

    // Método público para forçar a aplicação do brilho (útil se o overlay for criado dinamicamente)
    public void ForceApplyBrightness()
    {
        ApplySavedBrightness();
    }
}