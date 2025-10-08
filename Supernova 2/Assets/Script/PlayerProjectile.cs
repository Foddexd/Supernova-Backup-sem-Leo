using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 1;
    public string targetTag = "Enemy"; 

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Ignora camadas desnecessárias (ex: se player em layer "Player")
        Collider projCol = GetComponent<Collider>();
        if (projCol != null)
        {
            // Exemplo: Ignore layer do player (ajuste o layer ID no seu projeto)
            Physics.IgnoreLayerCollision(projCol.gameObject.layer, LayerMask.NameToLayer("Player"), true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log($"Projétil colidiu com: {other.name} (tag: {other.tag})"); 

        if (other.CompareTag("Player")) return;
       

        if (other.CompareTag("Player"))
        {
            return; 
        }

        
        if (other.CompareTag("ExplosiveBarrel"))
        {
            ExplosiveBarrel barrel = other.GetComponent<ExplosiveBarrel>();
            if (barrel != null)
            {
                barrel.TakeDamage(damage);
            }
            Destroy(gameObject); 
            return; 
        }

       
        if (other.CompareTag("Enemy"))
        {
            EnemyAi enemy = other.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Dano na vida do inimigo/boss
            }

           
            BossStun stun = other.GetComponent<BossStun>();
            if (stun != null)
            {
                stun.LevarTiro(); // Conta o tiro para stun
                Debug.Log("Tiro contado no boss! (via Enemy tag)"); 
            }

            Destroy(gameObject); 
            return; 
        }

        
        if (other.CompareTag("Boss"))
        {
            
            EnemyAi enemy = other.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

           
            BossStun stun = other.GetComponent<BossStun>();
            if (stun != null)
            {
                stun.LevarTiro();
                Debug.Log("Tiro contado no boss! (via Boss tag)"); 
            }

            Destroy(gameObject);
            return;
        }

        // Para qualquer outra colisão (ex: parede, chão), destrói a bala
        Destroy(gameObject);
    }
}