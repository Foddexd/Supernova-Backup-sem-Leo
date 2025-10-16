using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PegarKitMedico : MonoBehaviour
{
    public GameObject KitVisual;
    public GameObject KitInventario;
    public bool JogadorPerto = false;
    public int itemId;

    public GameObject texto;

    public float tempoExibicao = 2f;

    public GameObject textopegar;

    private OrdemInventario inventory;

    void Start()
    {
        // Encontra o script de invent�rio na cena
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("Script OrdemInventario n�o encontrado! Certifique-se de que ele esteja na cena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && KitVisual.activeSelf)
        {
            JogadorPerto = true;
            textopegar.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && KitVisual.activeSelf) 
        {
            JogadorPerto = false;
            textopegar.SetActive(false);

        }
    }


    void Update()
    {
        if (JogadorPerto && Input.GetKeyDown(KeyCode.E) && KitVisual.activeSelf)
        {
            textopegar.SetActive(false);
            KitVisual.SetActive(false);
            MostrarTexto();
            KitInventario.SetActive(true);
            // Adicionado: Chama o m�todo AddItem do invent�rio
            if (inventory != null)
            {
                inventory.AddItem(itemId);
                Debug.Log($"Kit M�dico (ID: {itemId}) adicionado ao invent�rio!");
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