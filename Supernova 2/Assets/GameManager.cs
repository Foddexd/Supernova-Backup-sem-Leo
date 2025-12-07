using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Nomes das Cenas Principais")]
    public string cenaAto1 = "Jogo Oficial";
    public string cenaAto2e3 = "Ato 2 e 3";
    public string cenaAto3 = "Ato 3";

    [Header("Cenas de Morte por Checkpoint")]
    public string cenaMorteCheckpoint1 = "GameOver1";
    public string cenaMorteCheckpoint2 = "GameOver2";
    public string cenaMorteCheckpoint3 = "GameOver3";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager iniciado");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(int checkpointLevel)
    {
        int current = PlayerPrefs.GetInt("CurrentCheckpoint", 1);

        if (checkpointLevel > current)
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", checkpointLevel);
            PlayerPrefs.Save();
            Debug.Log($"Checkpoint salvo: {checkpointLevel}");
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
        Debug.Log("Checkpoints resetados");
    }

    public void LoadCheckpointScene()
    {
        Time.timeScale = 1f;
        PlayerPrefs.Save();

        int checkpoint = GetCurrentCheckpoint();
        string sceneName = checkpoint switch
        {
            1 => cenaAto1,
            2 => cenaAto2e3,
            3 => cenaAto3,
            _ => cenaAto1
        };

        Debug.Log($"Carregando checkpoint {checkpoint}: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Método para quando o jogador morre
    public void PlayerDied()
    {
        int checkpoint = GetCurrentCheckpoint();
        string deathScene = checkpoint switch
        {
            1 => cenaMorteCheckpoint1,
            2 => cenaMorteCheckpoint2,
            3 => cenaMorteCheckpoint3,
            _ => cenaMorteCheckpoint1
        };

        Debug.Log($"Jogador morreu no checkpoint {checkpoint}. Indo para: {deathScene}");

        Time.timeScale = 1f;
        PlayerPrefs.Save();
        SceneManager.LoadScene(deathScene);
    }
}