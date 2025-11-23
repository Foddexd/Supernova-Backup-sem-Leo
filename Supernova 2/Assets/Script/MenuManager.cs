using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool canPause = true;
    public GameObject menuPausa;
    public GameObject controlsMenu; 
    private bool isMenuOpen = false;

    public static MenuManager instance;
    public void Awake() => instance = this;

    public PlayerShooting playerShooting;

    private void Start()
    {
        if (canPause)
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
               
                if (controlsMenu != null && controlsMenu.activeSelf)
                {
                    CloseControlsMenu();
                }
                else
                {
                   
                    isMenuOpen = !isMenuOpen;
                    menuPausa.SetActive(isMenuOpen);
                    if (!InventoryToggle.instance.IsInventoryOpen() && !DialogueManager.instance.IsFreezingDialogueOpen())
                    {
                        FreezeGame(isMenuOpen);
                    }

                    if (playerShooting != null)
                    {
                        playerShooting.enabled = !isMenuOpen;
                    }
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

  
    public void ResumeGame()
    {
        isMenuOpen = false;
        menuPausa.SetActive(false);
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(false); 
        }
        FreezeGame(false); 
        if (playerShooting != null)
        {
            playerShooting.enabled = true; 
        }
    }

   
    public void OpenControlsMenu()
    {
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(true);
            menuPausa.SetActive(false); 
        }
    }

    
    public void CloseControlsMenu()
    {
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(false); 
            menuPausa.SetActive(true); 
           
        }
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