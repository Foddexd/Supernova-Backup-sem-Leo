using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public int checkpointDestaCena = 1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void ReiniciarJogo()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadCheckpointScene();
        }
        else
        {
            int checkpoint = PlayerPrefs.GetInt("CurrentCheckpoint", 1);

            if (checkpoint == 1) SceneManager.LoadScene("Jogo Oficial");
            else if (checkpoint == 2) SceneManager.LoadScene("Ato 2 e 3");
            else if (checkpoint == 3) SceneManager.LoadScene("Ato 3");
            else SceneManager.LoadScene("Jogo Oficial");
        }
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}