using UnityEngine;

public class AbrirPuzzle : MonoBehaviour
{
    [Header("Referências do Puzzle")]
    public GameObject Puzzle;
    public GameObject TextoInteração;
    public KeyCode toggleKey = KeyCode.E;
    public PlayerShooting playerShooting;

    [Header("Dicas")]
    public GameObject dica1;
    public GameObject dica2;
    public GameObject dica3;

    [Header("Tempos (em segundos)")]
    public float tempoDica1 = 60f;
    public float tempoDica2 = 90f;
    public float tempoDica3 = 120f;

    [Header("Controle de Estado")]
    public bool puzzleResolvido = false;

    private bool PlayerNoTrigger = false;
    private bool PuzzleAberto = false;
    private bool firstPuzzleOpened = false;
    private bool timerActive = false;
    private float timer = 0f;

    private bool dica1Ativa = false;
    private bool dica2Ativa = false;
    private bool dica3Ativa = false;

    // ? novas flags — para não repetir dicas
    private bool dica1Mostrada = false;
    private bool dica2Mostrada = false;
    private bool dica3Mostrada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = true;
            TextoInteração.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = false;
            TextoInteração.SetActive(false);
        }
    }

    private void Update()
    {
        // Abre/fecha puzzle
        if (PlayerNoTrigger && Input.GetKeyDown(toggleKey))
        {
            OpenPuzzle(!PuzzleAberto);
        }

        // Controle do timer e dicas
        if (!puzzleResolvido)
        {
            // Timer ativo, jogo não pausado, puzzle fechado e nenhuma dica aberta
            if (timerActive && !IsGamePaused() && !PuzzleAberto && !dica1Ativa && !dica2Ativa && !dica3Ativa)
            {
                timer += Time.unscaledDeltaTime;

                if (timer >= tempoDica1 && !dica1Mostrada)
                {
                    MostrarDica(dica1, ref dica1Ativa);
                    dica1Mostrada = true; // ? marca que a dica já foi exibida
                }
                else if (timer >= tempoDica2 && !dica2Mostrada)
                {
                    MostrarDica(dica2, ref dica2Ativa);
                    dica2Mostrada = true;
                }
                else if (timer >= tempoDica3 && !dica3Mostrada)
                {
                    MostrarDica(dica3, ref dica3Ativa);
                    dica3Mostrada = true;
                }
            }

            // Fecha a dica com botão direito
            if (Input.GetMouseButtonDown(1))
            {
                if (dica1Ativa)
                    FecharDica(dica1, ref dica1Ativa);
                else if (dica2Ativa)
                    FecharDica(dica2, ref dica2Ativa);
                else if (dica3Ativa)
                    FecharDica(dica3, ref dica3Ativa);
            }
        }
    }

    private void MostrarDica(GameObject dica, ref bool dicaAtiva)
    {
        dica.SetActive(true);
        dicaAtiva = true;
        timerActive = false;

        if (playerShooting != null)
            playerShooting.enabled = false;

        MenuManager.instance.FreezeGame(true);
    }

    private void FecharDica(GameObject dica, ref bool dicaAtiva)
    {
        dica.SetActive(false);
        dicaAtiva = false;

        if (playerShooting != null)
            playerShooting.enabled = true;

        MenuManager.instance.FreezeGame(false);
        timerActive = true;
    }

    public void OpenPuzzle(bool abrir)
    {
        PuzzleAberto = abrir;
        Puzzle.SetActive(PuzzleAberto);

        if (playerShooting != null)
            playerShooting.enabled = !PuzzleAberto;

        if (!MenuManager.instance.IsMenuOpen() &&
            !InventoryToggle.instance.IsInventoryOpen() &&
            !DialogueManager.instance.IsFreezingDialogueOpen())
        {
            MenuManager.instance.FreezeGame(PuzzleAberto);
        }

        // Primeira vez que o puzzle é aberto
        if (abrir && !firstPuzzleOpened)
        {
            firstPuzzleOpened = true;
        }

        // Quando o puzzle é fechado pela primeira vez, inicia o timer
        if (!abrir && firstPuzzleOpened && !puzzleResolvido)
        {
            timerActive = true;
        }

        // Se abrir o puzzle novamente, pausa o timer
        if (abrir)
        {
            timerActive = false;
        }
    }

    public void ResolverPuzzle()
    {
        puzzleResolvido = true;
        timerActive = false;

        if (dica1Ativa) FecharDica(dica1, ref dica1Ativa);
        if (dica2Ativa) FecharDica(dica2, ref dica2Ativa);
        if (dica3Ativa) FecharDica(dica3, ref dica3Ativa);
    }

    private bool IsGamePaused()
    {
        return MenuManager.instance.IsMenuOpen() ||
               InventoryToggle.instance.IsInventoryOpen() ||
               DialogueManager.instance.IsFreezingDialogueOpen();
    }
}
