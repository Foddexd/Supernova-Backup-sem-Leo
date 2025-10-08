using UnityEngine;

public class StartDialogOnTrigger : MonoBehaviour
{
    public GameObject[] dialogos; // arraste todos os diálogos aqui em ordem
    public GameObject item1;
    public GameObject item2;

    public bool autoFreeze = false;

    private bool dialogoAtivo = false;
    private bool jaAtivado = false;
    private int indiceAtual = 0;

    private void Start()
    {
        DialogueManager.instance.AddDialogueTrigger(this);
    }

    void Update()
    {
        // só reage ao clique se o diálogo estiver ativo
        if (dialogoAtivo && Input.GetMouseButtonDown(1)) // botão esquerdo do mouse
        {
            AvancarDialogo();
        }
    }

    public bool IsFreezingAndOpen() => dialogoAtivo && autoFreeze;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !jaAtivado)
        {
            jaAtivado = true;
            dialogoAtivo = true;
            indiceAtual = 0;

            // ativa o primeiro diálogo
            if (dialogos.Length > 0)
                dialogos[0].SetActive(true);

            if(autoFreeze && !InventoryToggle.instance.IsInventoryOpen() && !MenuManager.instance.IsMenuOpen())
            {
                MenuManager.instance.FreezeGame(true);
            }
        }
    }

    private void AvancarDialogo()
    {
        // desativa o diálogo atual
        if (indiceAtual < dialogos.Length)
            dialogos[indiceAtual].SetActive(false);

        indiceAtual++;

        // se ainda há outro diálogo, mostra ele
        if (indiceAtual < dialogos.Length)
        {
            dialogos[indiceAtual].SetActive(true);
        }
        else
        {
            // terminou todos os diálogos
            dialogoAtivo = false;

            if (autoFreeze && !InventoryToggle.instance.IsInventoryOpen() && !MenuManager.instance.IsMenuOpen())
            {
                MenuManager.instance.FreezeGame(false);
            }

            if (item1) item1.SetActive(false);
            if (item2) item2.SetActive(false);
        }
    }
}