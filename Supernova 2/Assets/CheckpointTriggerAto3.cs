using UnityEngine;

public class CheckpointTriggerAto3 : MonoBehaviour
{
    [SerializeField] private int checkpointLevel = 3; // Checkpoint para Ato 3

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetCheckpoint(checkpointLevel);
            Debug.Log($"Checkpoint do Ato 3 ativado!");

            // Opcional: efeitos visuais/sonoros
            gameObject.SetActive(false);
        }
    }
}