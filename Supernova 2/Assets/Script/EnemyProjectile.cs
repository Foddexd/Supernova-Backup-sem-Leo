using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;
    public float overlapRadius = 0.25f; // fallback radius
    public LayerMask playerLayer; // configure no inspector para a layer do Player
    private bool hitSomething = false;

    void Start()
    {
        Collider c = GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();
        Debug.Log($"[EnemyProjectile] Start - name:{name}, collider:{(c != null ? c.GetType().Name : "null")}, isTrigger:{(c != null ? c.isTrigger : false)}, hasRigidbody:{(rb != null)}, rb.isKinematic:{(rb != null ? rb.isKinematic : false)}, layer:{LayerMask.LayerToName(gameObject.layer)}");
        if (playerLayer.value == 0)
            Debug.LogWarning("[EnemyProjectile] playerLayer não configurada! Configure a layer do Player no inspector (playerLayer).");
    }

    // Se o projétil usa trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[EnemyProjectile] OnTriggerEnter com: {other.name} (tag:{other.tag}, layer:{LayerMask.LayerToName(other.gameObject.layer)})");
        ProcessHit(other.gameObject);
    }

    // Se o projétil usa colisão física
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[EnemyProjectile] OnCollisionEnter com: {collision.gameObject.name} (tag:{collision.gameObject.tag}, layer:{LayerMask.LayerToName(collision.gameObject.layer)})");
        ProcessHit(collision.gameObject);
    }

    // Fallback por proximidade: detecta Player se nada mais funcionou
    void Update()
    {
        if (hitSomething) return;

        // Apenas faz a checagem se a layer do player foi configurada
        if (playerLayer.value != 0)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, overlapRadius, playerLayer);
            foreach (var col in hits)
            {
                Debug.Log($"[EnemyProjectile] OverlapSphere detectou: {col.name} (tag:{col.tag})");
                ProcessHit(col.gameObject);
                break;
            }
        }
    }

    private void ProcessHit(GameObject otherObj)
    {
        if (hitSomething) return;
        hitSomething = true;

        // Tenta achar PlayerHealth no objeto ou nos pais (caso o collider esteja em child)
        Transform t = otherObj.transform;
        PlayerHealth ph = null;
        while (t != null)
        {
            ph = t.GetComponent<PlayerHealth>();
            if (ph != null) break;
            t = t.parent;
        }

        if (otherObj.CompareTag("Player") || ph != null)
        {
            Debug.Log("[EnemyProjectile] Acertou jogador ou objeto com PlayerHealth. Aplicando dano se possível.");
            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("[EnemyProjectile] PlayerHealth.TakeDamage chamado.");
            }
            else
            {
                Debug.LogWarning("[EnemyProjectile] Tag 'Player' encontrada mas PlayerHealth NÃO encontrado no objeto ou pais.");
            }
        }
        else
        {
            Debug.Log("[EnemyProjectile] Não é player. Colidiu com: " + otherObj.name);
        }

        Destroy(gameObject);
    }

    // Visual debug no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }
}
