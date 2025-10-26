using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8;
    public GameObject item9;
    public GameObject item10;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Garante que apenas o jogador ativa
        {
            if (item1 != null) item1.SetActive(true);
            if (item2 != null) item2.SetActive(true);
            if (item3 != null) item3.SetActive(false);
            if (item4 != null) item4.SetActive(false);
            if (item5 != null) item5.SetActive(true);
            if (item6 != null) item6.SetActive(false);
            if (item7 != null) item7.SetActive(true);
            if (item8 != null) item8.SetActive(true);
            if (item9 != null) item9.SetActive(false);
            if (item10 != null) item10.SetActive(false);
          
          

        }
    }
}
