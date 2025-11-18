using UnityEngine;

public class StartDialogOnTrigger : MonoBehaviour
{
    public GameObject[] dialogos; // arraste todos os diálogos aqui em ordem
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject CartaoAto3;

    public bool autoFreeze = false;

    private bool dialogoAtivo = false;
    private bool jaAtivado = false;
    private int indiceAtual = 0;

    // 🔫 Referência ao PlayerShooting do jogador
    private PlayerShooting playerShootingRef;

    private void Start()
    {
        DialogueManager.instance.AddDialogueTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !jaAtivado)
        {
            jaAtivado = true;
            dialogoAtivo = true;
            indiceAtual = 0;

            // pega referência ao script de tiro do player
            playerShootingRef = other.GetComponent<PlayerShooting>();
            if (playerShootingRef == null)
                playerShootingRef = other.GetComponentInChildren<PlayerShooting>();

            // 🔫 DESATIVA O TIRO
            if (playerShootingRef != null)
            {
                playerShootingRef.enabled = false;
                Debug.Log("StartDialogOnTrigger: PlayerShooting DESATIVADO durante o diálogo.");
            }

            // ativa o primeiro diálogo
            if (dialogos.Length > 0)
                dialogos[0].SetActive(true);

            // freeze opcional
            if (autoFreeze &&
                !InventoryToggle.instance.IsInventoryOpen() &&
                !MenuManager.instance.IsMenuOpen())
            {
                MenuManager.instance.FreezeGame(true);
            }
        }
    }

    void Update()
    {
        if (dialogoAtivo && Input.GetMouseButtonDown(1))
        {
            AvancarDialogo();
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
            // terminou o diálogo
            dialogoAtivo = false;

            if (autoFreeze &&
                !InventoryToggle.instance.IsInventoryOpen() &&
                !MenuManager.instance.IsMenuOpen())
            {
                MenuManager.instance.FreezeGame(false);
            }

            // itens
            if (item1) item1.SetActive(false);
            if (item2) item2.SetActive(false);
            if (item3) item3.SetActive(true);
            if (CartaoAto3) CartaoAto3.SetActive(false);

            // 🔫 REATIVA O TIRO AO TERMINAR
            if (playerShootingRef != null)
            {
                playerShootingRef.enabled = true;
                Debug.Log("StartDialogOnTrigger: PlayerShooting REATIVADO após diálogo.");
            }
        }
    }

    public bool IsFreezingAndOpen() => dialogoAtivo && autoFreeze;
}
