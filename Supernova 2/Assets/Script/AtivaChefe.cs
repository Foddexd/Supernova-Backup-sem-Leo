using UnityEngine;
using UnityEngine.Events;

public class AtivadorPorTrigger : MonoBehaviour
{
    [Header("Objeto a ser ativado")]
    public GameObject objeto1;
    public GameObject objeto2;
    public GameObject objeto3;
    public GameObject objeto4;

    [Header("C�digo a ser executado")]
    public UnityEvent aoAtivar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objeto1.SetActive(false);
            objeto2.SetActive(false);
            objeto3.SetActive(true);
            objeto4.SetActive(true);

            aoAtivar.Invoke();

            
            
        }
    }
}
