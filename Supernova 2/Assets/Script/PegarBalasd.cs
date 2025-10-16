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
    private OrdemInventario inventory;  // Refer�ncia ao script de invent�rio

    private void Start()
    {
        ammoManager = FindObjectOfType<AmmoManager>();
        if (ammoManager == null)
        {
            Debug.LogWarning("PegarBalas: Nenhum AmmoManager encontrado.");
        }

        // Encontra o script de invent�rio na cena
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario n�o encontrado! Certifique-se de que ele esteja na cena.");
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

            // Adicionado: Chama o m�todo AddItem do invent�rio
            if (inventory != null)
            {
                inventory.AddItem(itemId);
                Debug.Log($"Balas (ID: {itemId}) adicionadas ao invent�rio!");
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