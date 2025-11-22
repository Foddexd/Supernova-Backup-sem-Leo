using UnityEngine;

public class Ler : MonoBehaviour
{
    private bool playerNoTrigger = false;
    private bool estaLendo = false;
    public GameObject FichaParaLer;
    public GameObject botaoInteracao;
    public GameObject texto;

    // Referência ao PlayerShooting para pausar/despausar
    private PlayerShooting playerShooting;

    private void Start()
    {
        // Desativa tudo no início
        if (FichaParaLer != null) FichaParaLer.SetActive(false);
        if (botaoInteracao != null) botaoInteracao.SetActive(false);
        if (texto != null) texto.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !estaLendo)
        {
            playerNoTrigger = true;

            // Pega referência do PlayerShooting
            playerShooting = other.GetComponent<PlayerShooting>();

            Debug.Log("Player entrou no trigger - Botão deve aparecer");

            // Mostra o botão de interação
            if (botaoInteracao != null)
            {
                botaoInteracao.SetActive(true);
                Debug.Log("Botão ativado com sucesso");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNoTrigger = false;
            Debug.Log("Player saiu do trigger");

            // Esconde o botão ao sair
            if (botaoInteracao != null) botaoInteracao.SetActive(false);
            if (texto != null) texto.SetActive(false);

            // Se estava lendo, fecha a nota
            if (estaLendo)
            {
                FecharNota();
            }
        }
    }

    void Update()
    {
        // Se player está no trigger e pressiona E
        if (playerNoTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!estaLendo)
            {
                AbrirNota();
            }
            else
            {
                FecharNota();
            }
        }
    }

    void AbrirNota()
    {
        Debug.Log("Abrindo nota...");
        estaLendo = true;

        // Mostra a ficha
        if (FichaParaLer != null) FichaParaLer.SetActive(true);

        // Esconde o botão
        if (botaoInteracao != null) botaoInteracao.SetActive(false);
        if (texto != null) texto.SetActive(false);

        // Pausa o jogo
        Time.timeScale = 0;

        // Desativa o script de tiro
        if (playerShooting != null)
        {
            playerShooting.enabled = false;
            Debug.Log("PlayerShooting desativado");
        }
    }

    void FecharNota()
    {
        Debug.Log("Fechando nota...");
        estaLendo = false;

        // Esconde a ficha
        if (FichaParaLer != null) FichaParaLer.SetActive(false);

        // Despausa o jogo
        Time.timeScale = 1;

        // Reativa o script de tiro
        if (playerShooting != null)
        {
            playerShooting.enabled = true;
            Debug.Log("PlayerShooting reativado");
        }

        // Se ainda está no trigger, mostra o botão novamente
        if (playerNoTrigger)
        {
            if (botaoInteracao != null)
            {
                botaoInteracao.SetActive(true);
                Debug.Log("Botão reativado após fechar nota");
            }
        }
    }
}