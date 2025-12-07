using UnityEngine;

public class DanoBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador atingido pelo Boss!");

            // Causa dano ao player (deixa o PlayerHealth lidar com a morte)
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.currentHealth);
            }
            else
            {
                Debug.LogError("PlayerHealth não encontrado!");
            }
        }
    }
}