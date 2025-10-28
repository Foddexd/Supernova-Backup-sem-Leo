using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public enum UpgradeType { CartuchoGrande, TiroDuplo }
    public UpgradeType tipoUpgrade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuma que o player tem a tag "Player"
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
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
                Destroy(gameObject); // Remove o item após coleta
            }
        }
    }
}