using UnityEngine;
using System.Collections;

public class MostraImagem : MonoBehaviour
{
   
    public GameObject imagem;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            imagem.SetActive(true);

            
            StartCoroutine(EsconderImagem());
        }
    }

    private IEnumerator EsconderImagem()
    {
        
        yield return new WaitForSeconds(2f);

        
        imagem.SetActive(false);

      
        Destroy(gameObject);
    }
}