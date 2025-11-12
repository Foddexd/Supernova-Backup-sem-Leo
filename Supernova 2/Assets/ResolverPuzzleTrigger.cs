using UnityEngine;

public class ResolverPuzzleTrigger : MonoBehaviour
{
    // Referência ao script AbrirPuzzle (arraste no Inspector)
    public AbrirPuzzle abrirPuzzleScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Chama a função para resolver o puzzle
            if (abrirPuzzleScript != null)
            {
                abrirPuzzleScript.ResolverPuzzle();
            }
        }
    }
}