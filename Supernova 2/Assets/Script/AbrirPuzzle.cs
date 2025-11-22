using UnityEngine;

public class AbrirPuzzle : MonoBehaviour
{
    [Header("Referências do Puzzle")]
    public GameObject Puzzle;
    public GameObject TextoInteracao;
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

    private bool playerNoTrigger = false;
    private bool puzzleAberto = false;
    private bool firstPuzzleOpened = false;
    private bool timerActive = false;
    private float timer = 0f;

    private bool dica1Ativa = false;
    private bool dica2Ativa = false;
    private bool dica3Ativa = false;

    private bool dica1Mostrada = false;
    private bool dica2Mostrada = false;
    private bool dica3Mostrada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNoTrigger = true;
            if (TextoInteracao != null)
                TextoInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNoTrigger = false;
            if (TextoInteracao != null)
                TextoInteracao.SetActive(false);
            if(Puzzle != null)
                Puzzle.SetActive(false);
            //ainda preciso fazer algo que conte como um tick , oq esta acontecendo é : eu preciso clicar mais uma vez o E fora para funcionar normalmente quando acontece o bug com ESQ
        }
    }

    private void Update()
    {
        // Abre ou fecha o puzzle
        if (playerNoTrigger && Input.GetKeyDown(toggleKey))
        {
            OpenPuzzle(!puzzleAberto);
        }

        // Controle do timer e exibição das dicas
        if (!puzzleResolvido)
        {
            if (timerActive && !IsGamePaused() && !puzzleAberto &&
                !dica1Ativa && !dica2Ativa && !dica3Ativa)
            {
                timer += Time.unscaledDeltaTime;

                if (timer >= tempoDica1 && !dica1Mostrada)
                {
                    MostrarDica(dica1, ref dica1Ativa);
                    dica1Mostrada = true;
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

            // Fecha a dica com o botão direito do mouse
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
        if (dica != null)
        {
            dica.SetActive(true);
            dicaAtiva = true;
            timerActive = false;

            if (playerShooting != null)
                playerShooting.enabled = false;

            MenuManager.instance.FreezeGame(true);
        }
    }

    private void FecharDica(GameObject dica, ref bool dicaAtiva)
    {
        if (dica != null)
        {
            dica.SetActive(false);
            dicaAtiva = false;

            if (playerShooting != null)
                playerShooting.enabled = true;

            MenuManager.instance.FreezeGame(false);
            timerActive = true;
        }
    }

    public void OpenPuzzle(bool abrir)
    {
        puzzleAberto = abrir;

        if (Puzzle != null)
            Puzzle.SetActive(puzzleAberto);

        if (playerShooting != null)
            playerShooting.enabled = !puzzleAberto;

        if (!MenuManager.instance.IsMenuOpen() &&
            !InventoryToggle.instance.IsInventoryOpen() &&
            !DialogueManager.instance.IsFreezingDialogueOpen())
        {
            MenuManager.instance.FreezeGame(puzzleAberto);
        }

        // Primeira vez que o puzzle é aberto
        if (abrir && !firstPuzzleOpened)
        {
            firstPuzzleOpened = true;
        }

        // Inicia o timer quando o puzzle é fechado pela primeira vez
        if (!abrir && firstPuzzleOpened && !puzzleResolvido)
        {
            timerActive = true;
        }

        // Pausa o timer quando o puzzle é reaberto
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
