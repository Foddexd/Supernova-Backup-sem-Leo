using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool canPause = true;
    public GameObject menuPausa;
    public GameObject controlsMenu;
    public GameObject configMenu;
    private bool isMenuOpen = false;

    public static MenuManager instance;

    [Header("Debug")]
    public Button reiniciarButton;
    public Button novoJogoButton;

    [Header("Audio References")]
    public VolumeSlider[] volumeSliders;
    public BrightnessSlider brightnessSlider;

    [Header("Configuration")]
    public bool autoRefreshSliders = true;

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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameOver")
        {
            enabled = false;
            Debug.Log("MenuManager desativado na cena GameOver");
            return;
        }
        if (canPause)
        {
            EnableCursor(false);
        }

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
                else if (configMenu != null && configMenu.activeSelf)
                {
                    CloseConfigMenu();
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
        if (configMenu != null)
        {
            configMenu.SetActive(false);
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

    public void OpenConfigMenu()
    {
        if (configMenu != null)
        {
            configMenu.SetActive(true);
            menuPausa.SetActive(false);

            if (autoRefreshSliders)
            {
                RefreshAllSliders();
            }
        }
    }

    public void CloseConfigMenu()
    {
        if (configMenu != null)
        {
            configMenu.SetActive(false);
            menuPausa.SetActive(true);
        }
    }

    public void RefreshAllSliders()
    {
        if (volumeSliders != null && volumeSliders.Length > 0)
        {
            foreach (var slider in volumeSliders)
            {
                if (slider != null)
                {
                    slider.RefreshSlider();
                }
            }
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.RefreshSlider();
        }
    }

    public void ResetToDefaultSettings()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ResetAudioSettings();
        }

        if (BrightnessManager.instance != null)
        {
            BrightnessManager.instance.ResetBrightness();
        }

        RefreshAllSliders();
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;

        if (GameManager.instance != null)
        {
            GameManager.instance.LoadCheckpointScene();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Time.timeScale = 1f;
        PlayerPrefs.Save();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void NovoJogo()
    {
        Time.timeScale = 1f;

        // Reseta checkpoints
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetCheckpoints();
        }
        else
        {
            PlayerPrefs.SetInt("CurrentCheckpoint", 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Jogo Oficial");
    }

    public void ReiniciarCenaSimples()
    {
        Debug.Log("Reiniciando cena simples...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}