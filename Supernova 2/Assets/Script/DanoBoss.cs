using UnityEngine;
using UnityEngine.SceneManagement; // Adicione esta linha

public class DanoBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Carrega a cena de morte antes de destruir
            SceneManager.LoadScene("GameOver");
            Destroy(other.gameObject);
        }
    }
}