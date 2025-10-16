using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PegarBalas : MonoBehaviour
{
    public GameObject BalaVisual;
    public GameObject BalaInventario;
    public bool JogadorPerto = false;
    public int itemId;  // Adicionado: ID do item (configure no Inspector)

    public GameObject texto;
    public float tempoExibicao = 2f;
    public GameObject textopegar;

    private AmmoManager ammoManager;
    private OrdemInventario inventory;  // Referência ao script de inventário

    private void Start()
    {
        ammoManager = FindObjectOfType<AmmoManager>();
        if (ammoManager == null)
        {
            Debug.LogWarning("PegarBalas: Nenhum AmmoManager encontrado.");
        }

        // Encontra o script de inventário na cena
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario não encontrado! Certifique-se de que ele esteja na cena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && BalaVisual.activeSelf)
        {
            JogadorPerto = true;
            textopegar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && BalaVisual.activeSelf)
        {
            JogadorPerto = false;
            textopegar.SetActive(false);
        }
    }

    void Update()
    {
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && BalaVisual.activeSelf)
        {
            textopegar.SetActive(false);
            BalaVisual.SetActive(false);
            MostrarTexto();

            if (ammoManager != null)
            {
                ammoManager.AdicionarCartucho();
            }

            // Adicionado: Chama o método AddItem do inventário
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
        texto.SetActive(true);
        yield return new WaitForSeconds(tempoExibicao);
        texto.SetActive(false);
    }
}