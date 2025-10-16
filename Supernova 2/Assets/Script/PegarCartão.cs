using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PegarCartão : MonoBehaviour
{
    public GameObject CabideComCartao;
    public GameObject CabideSemCartão;
    public GameObject CartãoInventario;
    public GameObject Botao;
    public bool JogadorPerto = false;
    public static bool TemCartao = false;
    public int itemId;  // Adicionado: ID do item (configure no Inspector)

    public GameObject texto;
    public float tempoExibicao = 2f;
    public GameObject textointeração;

    private OrdemInventario inventory;  // Referência ao script de inventário

    void Start()
    {
        // Encontra o script de inventário na cena
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario não encontrado! Certifique-se de que ele esteja na cena.");
        }
    }

    public void PegarCartao()
    {
        CabideComCartao.SetActive(false);
        CabideSemCartão.SetActive(true);
        CartãoInventario.SetActive(true);
        TemCartao = true;
        Botao.SetActive(false);

        // Adicionado: Chama o método AddItem do inventário
        if (inventory != null)
        {
            inventory.AddItem(itemId);
            Debug.Log($"Cartão (ID: {itemId}) adicionado ao inventário!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CabideComCartao.activeSelf)
        {
            JogadorPerto = true;
            textointeração.SetActive(true);
            Botao.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && CabideComCartao.activeSelf)
        {
            JogadorPerto = false;
            textointeração.SetActive(false);
            Botao.SetActive(false);
        }
    }

    void Update()
    {
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && CabideComCartao.activeSelf)
        {
            PegarCartao();
            textointeração.SetActive(false);
            MostrarTexto();
            Botao.SetActive(false);
        }
    }

    public void MostrarTexto()
    {
        StartCoroutine(ExibirTextoTemporario());
    }

    IEnumerator ExibirTextoTemporario()
    {
        texto.SetActive(true);
        yield return new WaitForSeconds(tempoExibicao);
        texto.SetActive(false);
    }
}