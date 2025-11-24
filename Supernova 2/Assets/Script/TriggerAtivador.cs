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
    public int checkpointLevel = 3; // 2 = Ato 2, 3 = Ato 3

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
                GameManager.instance.SetCheckpoint(checkpointLevel);
                Debug.Log($"Checkpoint ativado! Nível {checkpointLevel}");
            }
        }
    }
}