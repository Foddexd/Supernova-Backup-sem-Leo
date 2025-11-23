using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PegarCartão : MonoBehaviour
{
    public GameObject CabideComCartao;
    public GameObject CabideSemCartão;
    public GameObject Botao;
    public bool JogadorPerto = false;
    public int itemId = 1; // ID único para o cartão

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

        if (Botao != null) Botao.SetActive(false);
        if (textointeração != null) textointeração.SetActive(false);
    }

    public void PegarCartao()
    {
        CabideComCartao.SetActive(false);
        CabideSemCartão.SetActive(true);

        if (Botao != null) Botao.SetActive(false);
        if (textointeração != null) textointeração.SetActive(false);

        // Adiciona ao inventário - o visual será controlado pelo OrdemInventario
        if (inventory != null)
        {
            inventory.AddItem(itemId);
            Debug.Log($"Cartão (ID: {itemId}) adicionado ao inventário!");
        }

        Debug.Log("Cartão pego!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CabideComCartao.activeSelf && !inventory.HasItem(itemId))
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
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && CabideComCartao.activeSelf && !inventory.HasItem(itemId))
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