// Adicione este script ao DarkOverlay prefab
using UnityEngine;

public class DarkOverlayConfig : MonoBehaviour
{
    private void Awake()
    {
        // Garante que está no topo
        transform.SetAsLastSibling();

        // Garante que não bloqueie clicks
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}