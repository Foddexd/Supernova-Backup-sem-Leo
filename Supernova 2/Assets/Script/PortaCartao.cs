using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaCartao : MonoBehaviour
{
    public bool JogadorPerto;
    public GameObject PortaFechada1;
    public GameObject PortaFechada2;

    public GameObject imagemSemCartao;
    public GameObject imagemComCartao;

    private bool portaJaAberta = false;
    public int cartaoItemId = 1; // Mesmo ID do cartão

    private OrdemInventario inventory;

    void Start()
    {
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("OrdemInventario não encontrado!");
        }
    }

    public void AbrirPorta()
    {
        PortaFechada1.SetActive(false);
        PortaFechada2.SetActive(false);
        portaJaAberta = true;

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
            if (imagemSemCartao != null) imagemSemCartao.SetActive(false);
            if (imagemComCartao != null) imagemComCartao.SetActive(false);
        }
    }

    private void Update()
    {
        if (JogadorPerto && !portaJaAberta)
        {
            AtualizarImagens();

            // Verifica se tem cartão pelo inventário em vez de variável estática
            if (Input.GetKeyDown(KeyCode.E) && inventory.HasItem(cartaoItemId))
            {
                // Opcional: remover o cartão do inventário após usar
                // inventory.RemoveItem(cartaoItemId);
                AbrirPorta();
            }
        }
    }

    private void AtualizarImagens()
    {
        if (portaJaAberta) return;

        // Verifica se tem cartão pelo sistema de inventário
        bool temCartao = inventory.HasItem(cartaoItemId);

        if (!temCartao)
        {
            if (imagemSemCartao != null) imagemSemCartao.SetActive(true);
            if (imagemComCartao != null) imagemComCartao.SetActive(false);
        }
        else
        {
            if (imagemSemCartao != null) imagemSemCartao.SetActive(false);
            if (imagemComCartao != null) imagemComCartao.SetActive(true);
        }
    }
}