using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ler : MonoBehaviour
{
    private bool PlayerNoTrigger;
    private bool estaLendo = false;
    public GameObject FichaParaLer;
    public GameObject botaoInteracao;
    public GameObject texto;

    // Variável estática para rastrear se alguma nota está sendo lida
    public static bool IsReadingAnyNote = false;

    private void Start()
    {
        if (botaoInteracao != null)
            botaoInteracao.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = true;
            if (!estaLendo)
            {
                texto.SetActive(true);
                if (botaoInteracao != null)
                    botaoInteracao.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = false;
            texto.SetActive(false);
            if (estaLendo)
                AlternarLeitura();

            if (botaoInteracao != null)
                botaoInteracao.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerNoTrigger && Input.GetKeyDown(KeyCode.E))
        {
            AlternarLeitura();
        }
    }

    public void AlternarLeitura()
    {
        estaLendo = !estaLendo;
        if (estaLendo)
        {
            IsReadingAnyNote = true; // Marca que uma nota está sendo lida
            FichaParaLer.SetActive(true);
            Time.timeScale = 0;
            texto.SetActive(false);
            if (botaoInteracao != null)
                botaoInteracao.SetActive(false);
        }
        else
        {
            IsReadingAnyNote = false; // Marca que nenhuma nota está sendo lida (assumindo que apenas uma pode ser lida por vez)
            FichaParaLer.SetActive(false);
            Time.timeScale = 1;
            if (PlayerNoTrigger)
            {
                texto.SetActive(true);
                if (botaoInteracao != null)
                    botaoInteracao.SetActive(true);
            }
        }
    }
}