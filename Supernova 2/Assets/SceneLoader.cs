using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuração por Cena")]
    public bool desativarRedirecionamento = false;

    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GAMEMANAGER NÃO ENCONTRADO!");
            return;
        }

        Debug.Log($"=== SCENELOADER - {SceneManager.GetActiveScene().name} ===");

        if (!desativarRedirecionamento)
        {
            CheckSceneRedirect();
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
}