using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool canPause = true;
    public GameObject menuPausa;
    private bool isMenuOpen = false;

    public static MenuManager instance;
    public void Awake() => instance = this;

    private void Start()
    {
        if(canPause)
        {
            EnableCursor(false);
        }
    }

    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isMenuOpen = !isMenuOpen;
                menuPausa.SetActive(isMenuOpen);
                if (!InventoryToggle.instance.IsInventoryOpen() && !DialogueManager.instance.IsFreezingDialogueOpen())
                {
                    FreezeGame(isMenuOpen);
                }
            }
        }
    }

    public bool IsMenuOpen() => isMenuOpen;

    public bool IsPaused() => Time.timeScale == 0;

    public void FreezeGame(bool freeze = true)
    {
        //Debug.Log("Freeze: " + freeze);
        Time.timeScale = freeze ? 0 : 1;
        EnableCursor(freeze);
    }

    public void EnableCursor(bool cursorEnabled = true)
    {
        Cursor.lockState = cursorEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = cursorEnabled;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f; 
        Application.Quit();
        Debug.Log("Jogo encerrado"); 
    }
}
