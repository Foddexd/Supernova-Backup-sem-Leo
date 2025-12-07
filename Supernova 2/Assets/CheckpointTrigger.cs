using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointLevel = 1;
    [SerializeField] private bool debugLog = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (debugLog)
            {
                Debug.Log($"=== CHECKPOINT TRIGGER ATIVADO ===");
                Debug.Log($"Trigger: {name}, N�vel: {checkpointLevel}");
                Debug.Log($"Antes: PlayerPrefs = {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
            }

            // Chama o GameManager para salvar
            if (GameManager.instance != null)
            {
                GameManager.instance.SetCheckpoint(checkpointLevel);
            }
            else
            {
                // Fallback direto
                int current = PlayerPrefs.GetInt("CurrentCheckpoint", 1);
                if (checkpointLevel > current)
                {
                    PlayerPrefs.SetInt("CurrentCheckpoint", checkpointLevel);
                    PlayerPrefs.Save();
                    Debug.Log($"Checkpoint {checkpointLevel} salvo direto no PlayerPrefs");
                }
            }

            if (debugLog)
            {
                Debug.Log($"Depois: PlayerPrefs = {PlayerPrefs.GetInt("CurrentCheckpoint", 1)}");
            }

            // Desativa ap�s uso
            gameObject.SetActive(false);
        }
    }
}