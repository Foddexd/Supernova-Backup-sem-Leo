using UnityEngine;
using System.Collections.Generic; // Adicione isso para usar List<string>

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 7f; // Mudado de 5f para 7f: projéteis desaparecem após 7 segundos, mesmo sem colisão
    public int damage = 1;
    public string targetTag = "Enemy";

    // Lista de nomes de layers onde o projétil deve ser destruído
    // Configure no Inspector: adicione os nomes das layers (ex.: "Wall", "Ground")
    // O projétil só será destruído se colidir com um objeto nessa layer
    public List<string> destroyLayerNames = new List<string>();

    void Start()
    {
        Destroy(gameObject, lifetime); // Agora destrói após 7 segundos automaticamente

        // Ignora camadas desnecessárias (ex: se player em layer "Player")
        Collider projCol = GetComponent<Collider>();
        if (projCol != null)
        {
            // Exemplo: Ignore layer do player (ajuste o layer ID no seu projeto)
            Physics.IgnoreLayerCollision(projCol.gameObject.layer, LayerMask.NameToLayer("WhatIsPlayer"), true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Projétil colidiu com: {other.name} (tag: {other.tag}, layer: {LayerMask.LayerToName(other.gameObject.layer)})");

        // Ignora colisão com Player (como antes)
        if (other.CompareTag("Player")) return;

        // Aplica dano em tags específicas, INDEPENDENTEMENTE da layer (mantém o dano como solicitado)
        if (other.CompareTag("ExplosiveBarrel"))
        {
            ExplosiveBarrel barrel = other.GetComponent<ExplosiveBarrel>();
            if (barrel != null)
            {
                barrel.TakeDamage(damage);
            }
            // Não destrói aqui; só se a layer permitir
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
            // Não destrói aqui; só se a layer permitir
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
            // Não destrói aqui; só se a layer permitir
        }

        // Só destrói o projétil SE o nome da layer do objeto colidido estiver na lista destroyLayerNames
        string collidedLayerName = LayerMask.LayerToName(other.gameObject.layer);
        if (destroyLayerNames.Contains(collidedLayerName))
        {
            Destroy(gameObject);
        }
        // Se não estiver na lista, o projétil continua voando (não é destruído)
    }
}