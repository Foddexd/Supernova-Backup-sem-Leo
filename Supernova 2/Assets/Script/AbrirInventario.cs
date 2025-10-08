using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Objeto do inventário a ser ativado/desativado")]
    public GameObject inventoryUI;
    private bool isInventoryOpen = false;

    public static InventoryToggle instance;
    public void Awake() => instance = this;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public bool IsInventoryOpen() => isInventoryOpen;

    public void ToggleInventory()
    {
        if (isInventoryOpen)
        {
            if(!MenuManager.instance.IsMenuOpen() && !DialogueManager.instance.IsFreezingDialogueOpen())
            {
                MenuManager.instance.FreezeGame(false);
            }
        }
        else
        {
            MenuManager.instance.FreezeGame(true);
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
    }
}