using UnityEngine;

public class DebugReset : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== DEBUG RESET INICIADO ===");

        // Resetar imediatamente ao iniciar
        PlayerPrefs.DeleteKey("CurrentCheckpoint");
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs resetados para Ato 1");
        Debug.Log($"Checkpoint após reset: {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
    }

    void Update()
    {
        // Tecla F1 para reset manual
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.DeleteKey("CurrentCheckpoint");
            PlayerPrefs.Save();
            Debug.Log("CHECKPOINTS RESETADOS MANUALMENTE (F1)");
        }

        // Tecla F2 para debug
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log($"=== DEBUG ===");
            Debug.Log($"Cena: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Debug.Log($"Checkpoint: {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
            Debug.Log($"GameManager exists: {GameManager.instance != null}");
        }
    }
}