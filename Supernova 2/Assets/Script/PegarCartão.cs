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
    public int itemId;

    public GameObject texto;
    public float tempoExibicao = 2f;
    public GameObject textointeração;

    private OrdemInventario inventory;

    void Start()
    {
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario não encontrado!");
        }

        // Garante que o botão comece desativado
        if (Botao != null) Botao.SetActive(false);
        if (textointeração != null) textointeração.SetActive(false);
    }

    public void PegarCartao()
    {
        CabideComCartao.SetActive(false);
        CabideSemCartão.SetActive(true);
        CartãoInventario.SetActive(true);
        TemCartao = true;

        // Desativa TODOS os elementos de UI
        if (Botao != null) Botao.SetActive(false);
        if (textointeração != null) textointeração.SetActive(false);

        // Adiciona ao inventário
        if (inventory != null)
        {
            inventory.AddItem(itemId);
            Debug.Log($"Cartão (ID: {itemId}) adicionado ao inventário!");
        }

        Debug.Log("Cartão pego! TemCartao = " + TemCartao);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CabideComCartao.activeSelf && !TemCartao)
        {
            JogadorPerto = true;
            if (textointeração != null) textointeração.SetActive(true);
            if (Botao != null) Botao.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = false;
            if (textointeração != null) textointeração.SetActive(false);
            if (Botao != null) Botao.SetActive(false);
        }
    }

    void Update()
    {
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && CabideComCartao.activeSelf && !TemCartao)
        {
            PegarCartao();
            MostrarTexto();
        }
    }

    public void MostrarTexto()
    {
        StartCoroutine(ExibirTextoTemporario());
    }

    IEnumerator ExibirTextoTemporario()
    {
        if (texto != null) texto.SetActive(true);
        yield return new WaitForSeconds(tempoExibicao);
        if (texto != null) texto.SetActive(false);
    }
}