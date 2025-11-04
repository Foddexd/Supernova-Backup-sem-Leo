using UnityEngine;
using UnityEngine.UI; // Adicione isso se estiver usando UI Text

public class UpgradePickup : MonoBehaviour
{
    public enum UpgradeType { CartuchoGrande, TiroDuplo }
    public UpgradeType tipoUpgrade;

    // Referências para os objetos de UI
    public GameObject promptObject; // Objeto "aperte E para pegar" (ex: um TextMeshPro ou UI Text)
    public GameObject pickupMessage; // Objeto "voce pegou tal item" (ex: um TextMeshPro ou UI Text)
    public Text pickupText; // Componente Text do pickupMessage para personalizar o texto (opcional, se for UI Text)

    private bool playerInRange = false; // Flag para saber se o player está na área

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuma que o player tem a tag "Player"
        {
            playerInRange = true;
            if (promptObject != null)
            {
                promptObject.SetActive(true); // Ativa o prompt "aperte E para pegar"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptObject != null)
            {
                promptObject.SetActive(false); // Desativa o prompt ao sair da área
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

                // Mostrar mensagem de pickup
                if (pickupMessage != null)
                {
                    pickupMessage.SetActive(true);
                    if (pickupText != null)
                    {
                        pickupText.text = "Você pegou " + tipoUpgrade.ToString(); // Personaliza o texto
                    }
                }

                // Desativar o prompt e destruir o objeto
                if (promptObject != null)
                {
                    promptObject.SetActive(false);
                }
                Destroy(gameObject); // Remove o item após coleta
            }
        }
    }
}