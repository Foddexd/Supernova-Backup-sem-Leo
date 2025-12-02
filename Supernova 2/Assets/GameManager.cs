using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Nomes das Cenas - CONFIGURE AQUI!")]
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

            if (debugMode)
            {
                Debug.Log($"Cena Ato1: {cenaAto1}");
                Debug.Log($"Cena Ato2e3: {cenaAto2e3}");
                Debug.Log($"Cena Ato3: {cenaAto3}");
                Debug.Log($"Checkpoint salvo: {GetCurrentCheckpoint()}");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
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

        // SALVA todas as configurações antes de trocar de cena
        PlayerPrefs.Save();

        // Aplica configurações salvas antes de carregar
        if (AudioManager.instance != null)
        {
            AudioManager.instance.LoadAudioSettings();
        }

        if (BrightnessManager.instance != null)
        {
            BrightnessManager.instance.ApplyBrightness();
        }

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

    // Método chamado quando o jogador morre
    public void PlayerDied()
    {
        Debug.Log("Player morreu, recarregando checkpoint...");
        LoadCheckpointScene();
    }
}