using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuração")]
    public bool evitarRedirecionamentoAutomatico = false;

    private void Start()
    {
        Debug.Log($"=== SCENELOADER INICIADO ===");
        Debug.Log($"Cena atual: {SceneManager.GetActiveScene().name}");
        Debug.Log($"Checkpoint: {GameManager.instance.GetCurrentCheckpoint()}");

        if (!evitarRedirecionamentoAutomatico)
        {
            CheckSceneRedirect();
        }
        else
        {
            Debug.Log("Redirecionamento automático desativado");
        }
    }

    private void CheckSceneRedirect()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GAMEMANAGER NÃO ENCONTRADO!");
            return;
        }

        int currentCheckpoint = GameManager.instance.GetCurrentCheckpoint();
        string currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log($"Verificando redirecionamento: Cena='{currentSceneName}', Checkpoint={currentCheckpoint}");

        // Só redireciona se necessário
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

        if (shouldRedirect)
        {
            Debug.Log($"REDIRECIONANDO: {currentSceneName} -> {targetScene}");
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.Log("Nenhum redirecionamento necessário");
        }
    }
}