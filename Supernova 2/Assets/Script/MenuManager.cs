using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool canPause = true;
    public GameObject menuPausa;
    public GameObject controlsMenu;
    private bool isMenuOpen = false;

    public static MenuManager instance;

    [Header("Debug")]
    public Button reiniciarButton;
    public Button novoJogoButton;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerShooting playerShooting;

    private void Start()
    {
        if (canPause)
        {
            EnableCursor(false);
        }

        // Configurar botões com debug
        if (reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(() => {
                Debug.Log("BOTÃO REINICIAR CLICADO!");
                ReiniciarJogo();
            });
        }

        if (novoJogoButton != null)
        {
            novoJogoButton.onClick.AddListener(() => {
                Debug.Log("BOTÃO NOVO JOGO CLICADO!");
                NovoJogo();
            });
        }

        Debug.Log("=== MENU MANAGER INICIADO ===");
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
        Debug.Log("=== REINICIAR JOGO CHAMADO ===");

        // Verificar GameManager
        if (GameManager.instance == null)
        {
            Debug.LogError("GAMEMANAGER NÃO ENCONTRADO! Recarregando cena atual...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        Time.timeScale = 1f;
        Debug.Log("Chamando LoadCheckpointScene...");
        GameManager.instance.LoadCheckpointScene();
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void NovoJogo()
    {
        Debug.Log("=== NOVO JOGO CHAMADO ===");
        Time.timeScale = 1f;

        if (GameManager.instance != null)
        {
            GameManager.instance.ResetCheckpoints();
            SceneManager.LoadScene("Jogo Oficial");
        }
        else
        {
            Debug.LogError("GAMEMANAGER NÃO ENCONTRADO!");
            SceneManager.LoadScene("Jogo Oficial");
        }
    }

    // Método alternativo simples para testar
    public void ReiniciarCenaSimples()
    {
        Debug.Log("Reiniciando cena simples...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}