using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Nomes das Cenas")]
    public string cenaAto1 = "Jogo Oficial";
    public string cenaAto2e3 = "Ato 2 e 3";
    public string cenaAto3 = "Ato 3";

    [Header("Debug")]
    public bool debugMode = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("=== GAMEMANAGER INICIADO ===");
            Debug.Log($"Checkpoint atual: {GetCurrentCheckpoint()}");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // DEBUG: Log para verificar o estado
        if (debugMode)
        {
            Debug.Log("=== DEBUG GAMEMANAGER ===");
            Debug.Log($"Cena Ato1: {cenaAto1}");
            Debug.Log($"Cena Ato2e3: {cenaAto2e3}");
            Debug.Log($"Cena Ato3: {cenaAto3}");
            Debug.Log($"Checkpoint salvo: {GetCurrentCheckpoint()}");
        }
    }

    public void SetCheckpoint(int checkpointLevel)
    {
        int currentCheckpoint = GetCurrentCheckpoint();
        if (checkpointLevel > currentCheckpoint)
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", checkpointLevel);
            PlayerPrefs.Save();
            Debug.Log($"CHECKPOINT ATUALIZADO: Nível {checkpointLevel}");
        }
        else
        {
            Debug.Log($"Checkpoint ignorado: {checkpointLevel} (atual: {currentCheckpoint})");
        }
    }

    public int GetCurrentCheckpoint()
    {
        return PlayerPrefs.GetInt("CurrentCheckpoint", 1);
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.SetInt("CurrentCheckpoint", 1);
        PlayerPrefs.Save();
        Debug.Log("=== CHECKPOINTS RESETADOS ===");
    }

    public void LoadCheckpointScene()
    {
        Debug.Log("=== INICIANDO LOADCHECKPOINTSCENE ===");
        Time.timeScale = 1f;

        int checkpoint = GetCurrentCheckpoint();
        Debug.Log($"Checkpoint a carregar: {checkpoint}");

        string sceneName = "";

        switch (checkpoint)
        {
            case 1:
                sceneName = cenaAto1;
                break;
            case 2:
                sceneName = cenaAto2e3;
                break;
            case 3:
                sceneName = cenaAto3;
                break;
            default:
                sceneName = cenaAto1;
                break;
        }

        Debug.Log($"Carregando cena: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Método para debug no console
    [ContextMenu("Debug Checkpoint")]
    public void DebugCheckpoint()
    {
        Debug.Log($"=== DEBUG CHECKPOINT ===");
        Debug.Log($"Checkpoint atual: {GetCurrentCheckpoint()}");
        Debug.Log($"Cena atual: {SceneManager.GetActiveScene().name}");
    }

    [ContextMenu("Resetar Checkpoints")]
    public void ResetCheckpointsDebug()
    {
        ResetCheckpoints();
    }
}