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

    public void AbrirPorta()
    {
        PortaFechada1.SetActive(false);
        PortaFechada2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = false;
        }
    }

    private void Update()
    {
        // Mostra a imagem se o jogador estiver perto e não tiver o cartão
        if (JogadorPerto && !PegarCartão.TemCartao)
        {
            imagemSemCartao.SetActive(true);
        }
        else
        {
            imagemSemCartao.SetActive(false);
        }

        // Novo: Mostra a imagem se o jogador estiver perto, tiver o cartão e a porta ainda não foi aberta
        if (JogadorPerto && PegarCartão.TemCartao && !portaJaAberta)
        {
            imagemComCartao.SetActive(true);
        }
        else
        {
            imagemComCartao.SetActive(false);
        }

        // Mantém a lógica original para abrir a porta apenas se tiver o cartão e a porta ainda não foi aberta
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && PegarCartão.TemCartao == true && !portaJaAberta)
        {
            AbrirPorta();
            portaJaAberta = true;  // Novo: Marca que a porta foi aberta
        }
    }

    // Outro método (comentado, como no original)
    // private bool portaJaAberta = false;
    // private void Update()
    //{
    //   if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && PegarCartão.TemCartao && !portaJaAberta)
    //  {
    //        PortaFechada1.SetActive(false);
    //        PortaFechada2.SetActive(false);
    //        portaJaAberta = true;
    //  }
    // }
}