using UnityEngine;

public class AbrirPuzzle : MonoBehaviour
{
    public GameObject Puzzle;
    public GameObject TextoIntera��o;
    public KeyCode toggleKey = KeyCode.E;

    public bool PlayerNoTrigger = false;
    public bool PuzzleAberto = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = true;
            TextoIntera��o.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = false;
            TextoIntera��o.SetActive(false);

            OpenPuzzle(false);
        }
    }

    void Update()
    {
        if (PlayerNoTrigger && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle(!PuzzleAberto);
        }
    }

   
    public void OpenPuzzle(bool puzzleOpened)
    {
        PuzzleAberto = puzzleOpened;
        Puzzle.SetActive(PuzzleAberto);
        if (!MenuManager.instance.IsMenuOpen() && !InventoryToggle.instance.IsInventoryOpen() && !DialogueManager.instance.IsFreezingDialogueOpen())
        {
            MenuManager.instance.FreezeGame(PuzzleAberto);
        }
    }
}