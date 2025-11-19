using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Adicione isso se estiver usando UI Text

public class UpgradePickup : MonoBehaviour
{
    public enum UpgradeType { CartuchoGrande, TiroDuplo }
    public UpgradeType tipoUpgrade;

    // Referências para os objetos de UI
    public GameObject AperteE; // Objeto "aperte E para pegar" 
    public GameObject ImagemCanvas; // Imagem no canvas
    public Text pickupText; // Componente Text do pickupMessage para personalizar o texto (opcional, se for UI Text)

    // Variável pública para configurar o tempo de exibição da mensagem (em segundos)
    public float tempoExibicao = 2f;

    // Novo: Referência ao objeto visual do item (ex: o MeshRenderer ou o GameObject que representa o item no mundo)
    public GameObject itemVisual;

    private bool playerInRange = false; // Flag para saber se o player está na área

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuma que o player tem a tag "Player"
        {
            playerInRange = true;
            if (AperteE != null)
            {
                AperteE.SetActive(true); // Ativa o prompt "aperte E para pegar"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (AperteE != null)
            {
                AperteE.SetActive(false); // Desativa o prompt ao sair da área
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Player pressionou E dentro da área: aplicar upgrade
            PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>(); // Ou obtenha de outra forma se necessário
            if (playerShooting != null)
            {
                if (tipoUpgrade == UpgradeType.CartuchoGrande)
                {
                    playerShooting.AtivarUpgradeCartuchoGrande();
                }
                else if (tipoUpgrade == UpgradeType.TiroDuplo)
                {
                    playerShooting.AtivarUpgradeTiroDuplo();
                }

                // Chama a coroutine para mostrar a mensagem por tempoExibicao segundos e depois destruir
                StartCoroutine(MostrarMensagemPickup());
            }
        }
    }

    // Coroutine para mostrar a mensagem de pickup por tempoExibicao segundos e depois destruir o objeto
    private IEnumerator MostrarMensagemPickup()
    {
        // Novo: Desativa o objeto visual do item imediatamente (faz o item "sumir")
        if (itemVisual != null)
        {
            itemVisual.SetActive(false);
        }

        // Ativa a mensagem de pickup
        if (ImagemCanvas != null)
        {
            ImagemCanvas.SetActive(true);
            if (pickupText != null)
            {
                pickupText.text = "Você pegou " + tipoUpgrade.ToString(); // Personaliza o texto
            }
        }

        // Desativa o prompt
        if (AperteE != null)
        {
            AperteE.SetActive(false);
        }

        // Espera pelo tempo configurado
        yield return new WaitForSeconds(tempoExibicao);

        // Desativa a mensagem
        if (ImagemCanvas != null)
        {
            ImagemCanvas.SetActive(false);
        }

        // Destrói o objeto após a mensagem
        Destroy(gameObject);
    }
}