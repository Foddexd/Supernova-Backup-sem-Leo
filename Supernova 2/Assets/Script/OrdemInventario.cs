using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrdemInventario : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public Sprite[] itemIcons;

    private List<int> itemOrder; // A ordem dos itens (pode ter duplicatas)
    private int[] slotItemIds;   // Array que mapeia slot index para itemId (-1 para vazio)

    void Start()
    {
        itemOrder = new List<int>();
        slotItemIds = new int[inventorySlots.Length];
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            slotItemIds[i] = -1; // -1 significa vazio
        }

        foreach (GameObject slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.SetActive(false);
            }
        }
    }

    public void AddItem(int itemId)
    {
        if (itemId < 0 || itemId >= itemIcons.Length)
        {
            Debug.LogError("ItemId inválido!");
            return;
        }

        itemOrder.Add(itemId);

        // Encontra o primeiro slot vazio
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (slotItemIds[i] == -1) // Slot vazio
            {
                GameObject slot = inventorySlots[i];
                if (slot != null)
                {
                    Image image = slot.GetComponent<Image>();
                    if (image != null)
                    {
                        slot.SetActive(true);
                        image.sprite = itemIcons[itemId];
                        slotItemIds[i] = itemId;
                        Debug.Log($"Item adicionado: ID {itemId} no slot {i}");
                        return;
                    }
                }
            }
        }
        Debug.Log("Inventário cheio!");
    }

    // Remove o primeiro item com o itemId especificado
    public void RemoveItem(int itemId)
    {
        // Procura pelo item na lista de ordem (para saber a ordem de adição)
        // Mas note: a lista itemOrder tem a ordem, mas não sabemos qual slot
        // Vamos procurar no array slotItemIds pelo primeiro slot que tem o itemId
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            if (slotItemIds[i] == itemId)
            {
                // Encontrou, remove o slot
                GameObject slot = inventorySlots[i];
                if (slot != null)
                {
                    slot.SetActive(false);
                }
                slotItemIds[i] = -1;
                // Remove a primeira ocorrência na itemOrder
                itemOrder.Remove(itemId);
                Debug.Log($"Item removido: ID {itemId} do slot {i}");
                return;
            }
        }
        Debug.LogWarning($"Tentativa de remover item ID {itemId} que não está no inventário.");
    }

    // Verifica se o item está no inventário
    public bool HasItem(int itemId)
    {
        // Verifica se há pelo menos um slot com esse itemId
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            if (slotItemIds[i] == itemId)
                return true;
        }
        return false;
    }

    // Pega o slot onde o item está (primeira ocorrência)
    public GameObject GetItemSlot(int itemId)
    {
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            if (slotItemIds[i] == itemId)
                return inventorySlots[i];
        }
        return null;
    }
}