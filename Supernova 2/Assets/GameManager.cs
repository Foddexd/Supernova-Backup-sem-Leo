using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(int checkpointLevel)
    {
        // Só atualiza se for um checkpoint mais avançado
        int currentCheckpoint = PlayerPrefs.GetInt("CurrentCheckpoint", 1);
        if (checkpointLevel > currentCheckpoint)
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", checkpointLevel);
            PlayerPrefs.Save();
            Debug.Log($"Checkpoint salvo: Nível {checkpointLevel}");
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
        Debug.Log("Checkpoints resetados para o início");
    }

    public void LoadCheckpointScene()
    {
        Time.timeScale = 1f;
        int checkpoint = GetCurrentCheckpoint();

        switch (checkpoint)
        {
            case 1:
                SceneManager.LoadScene("Jogo Oficial"); // Ato 1, 2 e 3
                break;
            case 2:
                SceneManager.LoadScene("Ato 2 e 3"); // Ato 2 e 3
                break;
            case 3:
                SceneManager.LoadScene("Ato 3"); // Apenas Ato 3
                break;
            default:
                SceneManager.LoadScene("Jogo Oficial");
                break;
        }
    }
}