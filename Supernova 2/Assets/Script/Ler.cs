using UnityEngine;

public class Ler : MonoBehaviour
{
    private bool PlayerNoTrigger;
    private bool estaLendo = false;
    public GameObject FichaParaLer;
    public GameObject botaoInteracao; // Aperte E para interagir
    public GameObject texto; //Voce coletou 

    // Variável estática para rastrear se alguma nota está sendo lida
    public static bool IsReadingAnyNote = false;

    // Referência ao PlayerShooting do jogador que entrou no trigger
    private PlayerShooting playerShootingRef;

    private void Start()
    {
        if (botaoInteracao != null)
            botaoInteracao.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = true;

            // pega referência ao PlayerShooting (se houver)
            playerShootingRef = other.GetComponent<PlayerShooting>();
            if (playerShootingRef == null)
                Debug.LogWarning("Ler: Player entrou, mas não foi encontrado PlayerShooting no objeto 'Player'.");

            if (!estaLendo)
            {
                texto.SetActive(true);
                if (botaoInteracao != null)
                    botaoInteracao.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = false;
            texto.SetActive(false);

            // Se estava lendo e saiu do trigger, fechar leitura
            if (estaLendo)
                AlternarLeitura();

            if (botaoInteracao != null)
                botaoInteracao.SetActive(false);

            // opcional: limpa referência ao sair
            playerShootingRef = null;
        }
    }

    void Update()
    {
        if (PlayerNoTrigger && Input.GetKeyDown(KeyCode.E))
        {
            AlternarLeitura();
        }
    }

    public void AlternarLeitura()
    {
        estaLendo = !estaLendo;
        if (estaLendo)
        {
            IsReadingAnyNote = true; // marca que está lendo
            FichaParaLer.SetActive(true);
            Time.timeScale = 0;
            texto.SetActive(false);
            if (botaoInteracao != null) botaoInteracao.SetActive(false);

            // --- Desativa o script de tiro do jogador (mais confiável que só checar bool)
            if (playerShootingRef != null)
            {
                playerShootingRef.enabled = false;
                Debug.Log("Ler: PlayerShooting desativado enquanto lê.");
            }
            else
            {
                Debug.LogWarning("Ler: Não foi possível desativar PlayerShooting — referência nula.");
            }
        }
        else
        {
            IsReadingAnyNote = false; // marca que não está lendo
            FichaParaLer.SetActive(false);
            Time.timeScale = 1;
            if (PlayerNoTrigger)
            {
                texto.SetActive(true);
                if (botaoInteracao != null) botaoInteracao.SetActive(true);
            }

            // --- Reativa o script de tiro ao fechar a leitura
            if (playerShootingRef != null)
            {
                playerShootingRef.enabled = true;
                Debug.Log("Ler: PlayerShooting reativado após leitura.");
            }
        }
    }
}
