using UnityEngine;
using System.Collections;
using TMPro;

public class UsarKitMedico : MonoBehaviour
{
    public int kitMedicoItemId = 0; // ID do kit médico no inventário
    public PlayerHealth playerHealth;
    public GameObject piscaverde;
    public GameObject texto;
    public float tempoExibicao = 2f;

    private OrdemInventario inventory;

    void Start()
    {
        inventory = FindObjectOfType<OrdemInventario>();
        if (inventory == null)
        {
            Debug.LogError("OrdemInventario não encontrado!");
        }
    }

    void Update()
    {
        // Verifica se tem o kit médico no inventário em vez de um GameObject específico
        if (inventory.HasItem(kitMedicoItemId) && Input.GetKeyDown(KeyCode.C))
        {
            UsarKit();
        }
    }

    void UsarKit()
    {
        // Remove o kit do inventário
        inventory.RemoveItem(kitMedicoItemId);

        playerHealth.currentHealth = playerHealth.maxHealth;
        Debug.Log("Kit médico usado! Vida restaurada.");

        playerHealth.SendMessage("AtualizarIndicadoresDeVida");
        MostrarTexto();
        StartCoroutine(AtivarPiscadaVerde());
    }

    IEnumerator AtivarPiscadaVerde()
    {
        piscaverde.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        piscaverde.SetActive(false);
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