using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [Header("Configuração de Transição")]
    public float delayParaCarregar = 0.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarJogo()
    {
        // Pequeno delay para evitar clique acidental
        Invoke(nameof(CarregarCheckpoint), delayParaCarregar);
    }

    private void CarregarCheckpoint()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadCheckpointScene();
        }
        else
        {
            SceneManager.LoadScene("Jogo Oficial");
        }
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}