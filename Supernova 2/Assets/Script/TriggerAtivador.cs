using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Header("Configurações dos Itens")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8;
    public GameObject item9;
    public GameObject item10;

    [Header("Configurações do Checkpoint")]
    public bool isCheckpointt = false;
    public int checkpointLevel = 3;
    public bool debugLog = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ativa/desativa os itens
            if (item1 != null) item1.SetActive(true);
            if (item2 != null) item2.SetActive(true);
            if (item3 != null) item3.SetActive(false);
            if (item4 != null) item4.SetActive(false);
            if (item5 != null) item5.SetActive(true);
            if (item6 != null) item6.SetActive(false);
            if (item7 != null) item7.SetActive(true);
            if (item8 != null) item8.SetActive(true);
            if (item9 != null) item9.SetActive(false);
            if (item10 != null) item10.SetActive(false);

            // Sistema de checkpoint
            if (isCheckpointt)
            {
                if (debugLog)
                {
                    Debug.Log($"=== TRIGGER ACTIVATOR CHECKPOINT ===");
                    Debug.Log($"Trigger: {name}, Nível: {checkpointLevel}");
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
            }
        }
    }
}