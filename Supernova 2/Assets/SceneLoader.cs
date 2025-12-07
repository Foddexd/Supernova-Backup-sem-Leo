using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // Singleton instance
    public static SceneLoader instance;

    [Header("Configuração por Cena")]
    public bool desativarRedirecionamento = false;

    [Header("Configuração de Redirecionamento")]
    public bool redirecionarAutomaticamente = false; // NOVO: controle de redirecionamento

    [Header("Sistema de Brilho")]
    public GameObject darkOverlayPrefab;
    public bool criarOverlayBrilho = true;
    public string tagCanvasPrincipal = "MainCanvas";

    [Header("Configuração de Audio")]
    public bool configurarAudioAutomaticamente = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Debug.Log($"=== SCENELOADER - {SceneManager.GetActiveScene().name} ===");

        // Configura o sistema de brilho
        SetupBrightnessSystem();

        // Configura o sistema de audio
        if (configurarAudioAutomaticamente)
        {
            SetupAudioSystem();
        }

        // Verifica redirecionamento baseado em checkpoint (SOMENTE se ativado)
        if (redirecionarAutomaticamente && !desativarRedirecionamento)
        {
            CheckSceneRedirect();
        }
    }

    private void SetupAudioSystem()
    {
        // Garante que o AudioManager exista
        EnsureAudioManagerExists();

        // Coleta e configura todos os AudioSources da cena
        if (AudioManager.instance != null)
        {
            AudioManager.instance.CollectAllAudioSources();
            AudioManager.instance.LoadAudioSettings();
            Debug.Log("Sistema de áudio configurado.");
        }
    }

    private void EnsureAudioManagerExists()
    {
        if (AudioManager.instance == null)
        {
            AudioManager existingManager = FindObjectOfType<AudioManager>();
            if (existingManager == null)
            {
                GameObject managerObj = new GameObject("AudioManager");
                managerObj.AddComponent<AudioManager>();
                DontDestroyOnLoad(managerObj);
                Debug.Log("AudioManager criado automaticamente.");
            }
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
    }

    private void EnsureBrightnessManagerExists()
    {
        if (BrightnessManager.instance == null)
        {
            BrightnessManager existingManager = FindObjectOfType<BrightnessManager>();
            if (existingManager == null)
            {
                GameObject managerObj = new GameObject("BrightnessManager");
                managerObj.AddComponent<BrightnessManager>();
                DontDestroyOnLoad(managerObj);
                Debug.Log("BrightnessManager criado automaticamente.");
            }
        }
    }

    private void CreateDarkOverlay()
    {
        GameObject existingOverlay = GameObject.Find("DarkOverlay");
        if (existingOverlay != null)
        {
            Debug.Log("DarkOverlay já existe na cena.");

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

        GameObject canvasObj = GameObject.FindGameObjectWithTag(tagCanvasPrincipal);
        if (canvasObj == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                canvasObj = canvas.gameObject;
            }
        }

        if (canvasObj != null)
        {
            GameObject overlayObj = Instantiate(darkOverlayPrefab, canvasObj.transform);
            overlayObj.name = "DarkOverlay";

            RectTransform rectTransform = overlayObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }

            overlayObj.transform.SetAsLastSibling();

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
            StartCoroutine(ApplyBrightnessDelayed());
        }
    }

    private IEnumerator ApplyBrightnessDelayed()
    {
        yield return null;

        BrightnessManager.instance.FindDarkOverlay();
        BrightnessManager.instance.ApplyBrightness();
        Debug.Log("Configurações de brilho aplicadas.");
    }

    // MÉTODO DE REDIRECIONAMENTO - MELHORADO E SEGURO
    private void CheckSceneRedirect()
    {
        // Verifica se o GameManager existe
        if (GameManager.instance == null)
        {
            Debug.LogWarning("GameManager não encontrado para redirecionamento.");
            return;
        }

        int currentCheckpoint = GameManager.instance.GetCurrentCheckpoint();
        string currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log($"Verificando redirecionamento: Cena='{currentSceneName}', Checkpoint={currentCheckpoint}");

        // Evita redirecionamento se já estiver na cena correta
        if ((currentSceneName == GameManager.instance.cenaAto1 && currentCheckpoint == 1) ||
            (currentSceneName == GameManager.instance.cenaAto2e3 && currentCheckpoint == 2) ||
            (currentSceneName == GameManager.instance.cenaAto3 && currentCheckpoint == 3))
        {
            Debug.Log("Já está na cena correta para o checkpoint atual.");
            return;
        }

        // Redireciona apenas se necessário e se for uma cena de jogo
        bool shouldRedirect = false;
        string targetScene = "";

        if (currentSceneName == GameManager.instance.cenaAto1 && currentCheckpoint >= 2)
        {
            shouldRedirect = true;
            targetScene = GameManager.instance.cenaAto2e3;
        }
        else if (currentSceneName == GameManager.instance.cenaAto2e3 && currentCheckpoint >= 3)
        {
            shouldRedirect = true;
            targetScene = GameManager.instance.cenaAto3;
        }
        // NOTA: Não redireciona da cena Ato3 porque não tem para onde ir

        if (shouldRedirect)
        {
            Debug.Log($"REDIRECIONANDO: {currentSceneName} -> {targetScene}");

            // Pequeno delay para garantir que tudo está salvo
            StartCoroutine(RedirectWithDelay(targetScene, 0.1f));
        }
        else
        {
            Debug.Log("Nenhum redirecionamento necessário ou permitido.");
        }
    }

    private IEnumerator RedirectWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    // NOVO: Método para forçar verificação de redirecionamento
    public void ForçarVerificacaoRedirecionamento()
    {
        if (GameManager.instance != null)
        {
            CheckSceneRedirect();
        }
    }

    // NOVO: Método para verificar se precisa redirecionar sem executar
    public bool PrecisaRedirecionar()
    {
        if (GameManager.instance == null) return false;

        int currentCheckpoint = GameManager.instance.GetCurrentCheckpoint();
        string currentSceneName = SceneManager.GetActiveScene().name;

        return (currentSceneName == GameManager.instance.cenaAto1 && currentCheckpoint >= 2) ||
               (currentSceneName == GameManager.instance.cenaAto2e3 && currentCheckpoint >= 3);
    }

    // Método público para forçar a aplicação do brilho
    public void ForceApplyBrightness()
    {
        ApplySavedBrightness();
    }

    // NOVO: Método para configurar manualmente o redirecionamento
    public void SetRedirecionamento(bool ativo)
    {
        redirecionarAutomaticamente = ativo;
        Debug.Log($"Redirecionamento automático: {(ativo ? "ATIVADO" : "DESATIVADO")}");
    }
}