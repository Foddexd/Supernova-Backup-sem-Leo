using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AtivadorCameras : MonoBehaviour

 {
   public GameObject CameraParada;
   public GameObject MainCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Garante que apenas o jogador ativa
        {
            if (CameraParada != null) CameraParada.SetActive(true);
            if (MainCamera != null) MainCamera.SetActive(false);
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Garante que apenas o jogador ativa
        {
            if (CameraParada != null) CameraParada.SetActive(false);
            if (MainCamera != null) MainCamera.SetActive(true);
          

        }

    }
}
