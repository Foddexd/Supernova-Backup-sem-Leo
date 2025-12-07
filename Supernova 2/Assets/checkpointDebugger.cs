using UnityEngine;

public class CheckpointDebugger : MonoBehaviour
{
    void Update()
    {
        // F1 - Mostrar info
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log($"=== DEBUG ===");
            Debug.Log($"Cena: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Debug.Log($"PlayerPrefs: {PlayerPrefs.GetInt("CurrentCheckpoint", -1)}");

            if (GameManager.instance != null)
            {
                Debug.Log($"GameManager: {GameManager.instance.GetCurrentCheckpoint()}");
            }
        }

        // F2 - Resetar para 1
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", 1);
            PlayerPrefs.Save();
            Debug.Log("Checkpoint FORÇADO para 1");
        }

        // F3 - Forçar checkpoint 2
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", 2);
            PlayerPrefs.Save();
            Debug.Log("Checkpoint FORÇADO para 2");
        }

        // F4 - Forçar checkpoint 3
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", 3);
            PlayerPrefs.Save();
            Debug.Log("Checkpoint FORÇADO para 3");
        }

        // F5 - Mostrar PlayerPrefs
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log($"=== PLAYERPREFS ===");
            Debug.Log($"Tem 'CurrentCheckpoint'? {PlayerPrefs.HasKey("CurrentCheckpoint")}");
            Debug.Log($"Valor: {PlayerPrefs.GetInt("CurrentCheckpoint", -1)}");
            Debug.Log($"Cena: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
    }
}