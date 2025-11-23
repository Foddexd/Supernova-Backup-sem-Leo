using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PegarBalas : MonoBehaviour
{
    public GameObject BalaVisual;
    public bool JogadorPerto = false;
    public int itemId;  // ID do item (configure no Inspector)

    public GameObject texto;
    public float tempoExibicao = 2f;
    public GameObject textopegar;

    private AmmoManager ammoManager;
    private OrdemInventario inventory;

    private void Start()
    {
        ammoManager = FindObjectOfType<AmmoManager>();
        if (ammoManager == null)
        {
            Debug.LogWarning("PegarBalas: Nenhum AmmoManager encontrado.");
        }

        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario não encontrado!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && BalaVisual.activeSelf)
        {
            JogadorPerto = true;
            if (textopegar != null) textopegar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JogadorPerto = false;
            if (textopegar != null) textopegar.SetActive(false);
        }
    }

    void Update()
    {
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && BalaVisual.activeSelf)
        {
            if (textopegar != null) textopegar.SetActive(false);
            BalaVisual.SetActive(false);
            MostrarTexto();

            // Adiciona munição ao AmmoManager
            if (ammoManager != null)
            {
                ammoManager.AdicionarCartucho();
            }

            // Adiciona ao inventário visual
            if (inventory != null)
            {
                inventory.AddItem(itemId);
                Debug.Log($"Balas (ID: {itemId}) adicionadas ao inventário!");
            }
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