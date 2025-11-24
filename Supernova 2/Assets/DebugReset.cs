using UnityEngine;

public class DebugReset : MonoBehaviour
{
    [Header("Configuração")]
    public bool resetarAoIniciar = true;
    public bool habilitarTeclasDebug = true;

    void Start()
    {
        Debug.Log($"=== DEBUG RESET INICIADO - {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} ===");

        if (resetarAoIniciar)
        {
            // Resetar imediatamente ao iniciar
            PlayerPrefs.DeleteKey("CurrentCheckpoint");
            PlayerPrefs.Save();

            Debug.Log("PlayerPrefs resetados para Ato 1");
            Debug.Log($"Checkpoint após reset: {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
        }
        else
        {
            Debug.Log($"Checkpoint atual: {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
        }
    }

    void Update()
    {
        if (!habilitarTeclasDebug) return;

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

        // Tecla F3 para carregar Ato 1
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Jogo Oficial");
        }

        // Tecla F4 para carregar Ato 2 e 3
        if (Input.GetKeyDown(KeyCode.F4))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ato 2 e 3");
        }

        // Tecla F5 para carregar Ato 3
        if (Input.GetKeyDown(KeyCode.F5))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ato 3");
        }
    }

    // Método para UI - botão de reset
    public void ResetarCheckpointsUI()
    {
        PlayerPrefs.DeleteKey("CurrentCheckpoint");
        PlayerPrefs.Save();
        Debug.Log("Checkpoints resetados via UI");
    }
}