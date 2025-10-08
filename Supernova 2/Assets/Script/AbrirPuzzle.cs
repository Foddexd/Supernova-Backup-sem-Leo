using UnityEngine;

public class AbrirPuzzle : MonoBehaviour
{
    public GameObject Puzzle;
    public GameObject TextoInteração;
    public KeyCode toggleKey = KeyCode.E;

    public bool PlayerNoTrigger = false;
    public bool PuzzleAberto = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = true;
            TextoInteração.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNoTrigger = false;
            TextoInteração.SetActive(false);

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