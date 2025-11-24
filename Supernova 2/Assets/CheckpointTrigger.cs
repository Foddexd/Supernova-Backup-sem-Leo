using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointLevel = 2; // Checkpoint para Ato 2

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetCheckpoint(checkpointLevel);
            Debug.Log($"Checkpoint do Ato 2 ativado!");

            // Opcional: efeitos visuais/sonoros
            gameObject.SetActive(false);
        }
    }
}