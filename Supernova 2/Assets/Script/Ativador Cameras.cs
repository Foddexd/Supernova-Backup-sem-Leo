using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AtivadorCameras : MonoBehaviour

 {
   public GameObject item1;
   public GameObject item2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Garante que apenas o jogador ativa
        {
            if (item1 != null) item1.SetActive(true);
            if (item2 != null) item2.SetActive(false);
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Garante que apenas o jogador ativa
        {
            if (item1 != null) item1.SetActive(false);
            if (item2 != null) item2.SetActive(true);
          

        }

    }
}
