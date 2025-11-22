using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaCartao : MonoBehaviour
{
    public bool JogadorPerto;
    public GameObject PortaFechada1;
    public GameObject PortaFechada2;

    // Referência para a imagem que aparece quando não tem cartão
    public GameObject imagemSemCartao;

    // Novo: Referência para a imagem que aparece quando tem cartão e a porta ainda não foi aberta
    public GameObject imagemComCartao;

    // Novo: Flag para verificar se a porta já foi aberta
    private bool portaJaAberta = false;

    // Referência ao script PegarCartão para verificar se o cartão foi pego
    private PegarCartão pegarCartaoScript;

    void Start()
    {
        // Encontra o script PegarCartão na cena
        pegarCartaoScript = FindObjectOfType<PegarCartão>();
    }

    public void AbrirPorta()
    {
        PortaFechada1.SetActive(false);
        PortaFechada2.SetActive(false);
        portaJaAberta = true;

        // Desativa ambas as imagens quando a porta é aberta
        if (imagemSemCartao != null) imagemSemCartao.SetActive(false);
        if (imagemComCartao != null) imagemComCartao.SetActive(false);

        Debug.Log("Porta aberta com cartão!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = true;
            AtualizarImagens();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = false;
            // Desativa todas as imagens quando o jogador sai
            if (imagemSemCartao != null) imagemSemCartao.SetActive(false);
            if (imagemComCartao != null) imagemComCartao.SetActive(false);
        }
    }

    private void Update()
    {
        // Só processa se o jogador está perto e a porta não foi aberta
        if (JogadorPerto && !portaJaAberta)
        {
            AtualizarImagens();

            // Abre a porta se tiver cartão e pressionar E
            if (Input.GetKeyDown(KeyCode.E) && PegarCartão.TemCartao)
            {
                AbrirPorta();
            }
        }
    }

    // Método separado para atualizar as imagens
    private void AtualizarImagens()
    {
        if (portaJaAberta) return;

        // Se não tem cartão, mostra imagem sem cartão
        if (!PegarCartão.TemCartao)
        {
            if (imagemSemCartao != null) imagemSemCartao.SetActive(true);
            if (imagemComCartao != null) imagemComCartao.SetActive(false);
        }
        // Se tem cartão, mostra imagem com cartão
        else
        {
            if (imagemSemCartao != null) imagemSemCartao.SetActive(false);
            if (imagemComCartao != null) imagemComCartao.SetActive(true);
        }
    }
}