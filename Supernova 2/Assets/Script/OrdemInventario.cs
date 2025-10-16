using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Para usar List

public class OrdemInventario : MonoBehaviour
{
    public GameObject[] inventorySlots; // Array de GameObjects dos slots (configure na ordem visual na Unity Editor)
    public Sprite[] itemIcons;          // Array de sprites dos itens (cada �ndice corresponde a um itemId)

    private List<int> itemOrder; // Lista para rastrear a ordem dos itens (armazena os itemIds na ordem que foram adicionados)

    void Start()
    {
        // Inicializa a lista de itens (opcional, mas ajuda a gerenciar a ordem)
        itemOrder = new List<int>();

        // Garante que todos os slots comecem inativos (se n�o estiverem configurados assim no Editor)
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
        // Primeiro, verifica se o itemId � v�lido (evita erros de �ndice no array itemIcons)
        if (itemId < 0 || itemId >= itemIcons.Length)
        {
            Debug.LogError("ItemId inv�lido! Verifique o array itemIcons.");
            return;
        }

        // Adiciona o itemId � lista para manter a ordem
        itemOrder.Add(itemId);

        // Percorre os slots para encontrar o primeiro inativo
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject slot = inventorySlots[i];

            if (slot != null && !slot.activeSelf) // Verifica se o slot existe e est� inativo
            {
                Image image = slot.GetComponent<Image>(); // Obt�m o componente Image

                if (image != null) // Verifica se o componente Image existe
                {
                    slot.SetActive(true); // Ativa o slot
                    image.sprite = itemIcons[itemId]; // Define o sprite do item
                    Debug.Log($"Item adicionado: ID {itemId} no slot {i}"); // Log para depura��o
                    return; // Sai ap�s adicionar o item
                }
                else
                {
                    Debug.LogError($"Slot {i} n�o tem um componente Image! Verifique no Unity Editor.");
                    return; // Sai se n�o houver Image
                }
            }
        }

        Debug.Log("Invent�rio cheio! N�o h� slots dispon�veis.");
    }

    // M�todo opcional para depura��o: Exibe a ordem dos itens no console
    public void DebugItemOrder()
    {
        Debug.Log("Ordem dos itens no invent�rio:");
        foreach (int id in itemOrder)
        {
            Debug.Log($"Item ID: {id}");
        }
    }
}