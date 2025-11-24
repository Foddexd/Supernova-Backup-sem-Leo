using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        // Verifica se precisa redirecionar baseado no checkpoint
        CheckSceneRedirect();
    }

    private void CheckSceneRedirect()
    {
        int currentCheckpoint = GameManager.instance.GetCurrentCheckpoint();
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Se está em uma cena anterior ao checkpoint, redireciona
        if (currentSceneName == "Jogo Oficial" && currentCheckpoint >= 2)
        {
            GameManager.instance.LoadCheckpointScene();
        }
        else if (currentSceneName == "Ato 2 e 3" && currentCheckpoint >= 3)
        {
            GameManager.instance.LoadCheckpointScene();
        }
    }
}