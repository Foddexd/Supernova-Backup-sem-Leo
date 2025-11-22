using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; // onde o tiro nasce (alinhado com a arma/câmera)
    public Camera mainCamera;
    public bool temArma = false;

    [Header("Munição")]
    public int maxBalasPorCartucho = 30; // Padrão: 30. O upgrade pode garantir ou aumentar isso.
    public int balasNoCartucho = 30;

    public TextMeshProUGUI balasTexto;
    public TextMeshProUGUI cartuchosTexto;

    private AmmoManager ammoManager;

    public GameObject Som;
    public GameObject Luz;

    // Novos campos para upgrades
    [Header("Upgrades")]
    public bool upgradeCartuchoGrande = false; // Upgrade para cartucho de 30 balas
    public bool upgradeTiroDuplo = false; // Upgrade para atirar dois tiros

    //itens que vão se desativar com o tiro 
    public AudioSource audioSource;  // Referência ao AudioSource
    public Light muzzleLight;        // Referência à luz
    public float lightDuration = 0.1f;  // Duração do flash em segundos (ajuste para mais rápido/menos)

    void Start()
    {
        ammoManager = GetComponent<AmmoManager>()
                   ?? GetComponentInParent<AmmoManager>()
                   ?? FindObjectOfType<AmmoManager>();

        if (ammoManager == null)
            Debug.LogWarning("PlayerShooting: nenhum AmmoManager encontrado. Pickup pode não funcionar.");

        if (temArma) balasNoCartucho = maxBalasPorCartucho;
        AtualizarUI();

        // Verificação de setup (adicionei para debug)
        if (mainCamera == null) Debug.LogError("PlayerShooting: Atribua a mainCamera no Inspector!");
        if (firePoint == null) Debug.LogError("PlayerShooting: Atribua o firePoint no Inspector!");
    }

    void Update()
    {
        // ATENÇÃO: Este script será desativado/ativado pelo script Ler
        // então não precisa verificar IsReadingAnyNote

        // Apenas atualiza a UI continuamente
        AtualizarUI();

#if !UNITY_ANDROID && !UNITY_IOS
        // Processa inputs normalmente - o script será desativado quando ler notas
        if (temArma && Input.GetMouseButtonDown(0)) TentarAtirar();
        if (temArma && Input.GetKeyDown(KeyCode.R)) Recarregar();
#endif
    }

    public void TentarAtirar()
    {
        if (balasNoCartucho > 0)
        {
            Shoot();
            balasNoCartucho--;
            PlayShootEffects();  // Chama os efeitos de som e luz aqui
        }
        else if (ammoManager != null && ammoManager.GetCartuchos() > 0)
        {
            Debug.Log("Sem balas no cartucho! Pressione R para recarregar.");
        }
        else
        {
            Debug.Log("Acabaram todas as balas e cartuchos!");
        }
    }
    // Nova função para tocar som e piscar luz (evita poluição)
    void PlayShootEffects()
    {
        // Toca o som uma vez (sem sobreposição)
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);  // Toca o clip atribuído no AudioSource
        }
        // Ativa a luz e desativa após lightDuration segundos
        if (muzzleLight != null)
        {
            muzzleLight.enabled = true;
            Invoke("DisableLight", lightDuration);  // Chama DisableLight automaticamente
        }
    }

    void DisableLight()
    {
        if (muzzleLight != null)
        {
            muzzleLight.enabled = false;
        }
    }

    void Shoot()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Shoot: mainCamera não atribuída!");
            return;
        }
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro da tela
        Vector3 direction = ray.direction;
        float shootSpeed = 40f;
        float spawnOffset = 0.5f;
        Vector3 spawnPos = firePoint.position + direction * spawnOffset;
        if (upgradeTiroDuplo)
        {
            // Atira dois projéteis com offset lateral pequeno para simular dispersão
            Vector3 rightOffset = mainCamera.transform.right * 0.1f; // Offset para a direita (aumente para mais dispersão, ex.: 0.2f)
                                                                     // Primeiro tiro (original)
            GameObject proj1 = InstanciarProjétil(spawnPos, direction, shootSpeed);
            // Segundo tiro (com offset)
            Vector3 offsetDirection = (direction + rightOffset * 0.5f).normalized; // Pequena inclinação
            Vector3 offsetSpawnPos = firePoint.position + offsetDirection * spawnOffset;
            GameObject proj2 = InstanciarProjétil(offsetSpawnPos, offsetDirection, shootSpeed);
            // Ignorar colisão entre os dois projéteis para evitar que se "batam"
            Collider collider1 = proj1.GetComponent<Collider>();
            Collider collider2 = proj2.GetComponent<Collider>();
            if (collider1 != null && collider2 != null)
            {
                Physics.IgnoreCollision(collider1, collider2, true);
            }
        }
        else
        {
            // Tiro único (padrão)
            InstanciarProjétil(spawnPos, direction, shootSpeed);
        }
        Debug.Log($"Tiro disparado! Upgrade Tiro Duplo: {upgradeTiroDuplo}, Posição: {spawnPos}");
    }
    // Método auxiliar para instanciar projétil (evita duplicação de código)
    private GameObject InstanciarProjétil(Vector3 spawnPos, Vector3 direction, float shootSpeed)
    {
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
        Collider projCollider = proj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (projCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(projCollider, playerCollider, true);
        }
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * shootSpeed;
        }
        else
        {
            Debug.LogError("Projétil sem Rigidbody! Adicione um no prefab.");
        }
        return proj; // Retorna o projétil para uso no IgnoreCollision
    }
    void Recarregar()
    {
        if (balasNoCartucho == maxBalasPorCartucho)
        {
            Debug.Log("Cartucho cheio.");
            return;
        }

        if (ammoManager != null && ammoManager.ConsumirCartucho())
        {
            Debug.Log("Recarregando... Perdeu as balas restantes.");
            balasNoCartucho = maxBalasPorCartucho; // Com upgrade, será 30 se ativado
        }
        else
        {
            Debug.Log("Sem cartuchos sobrando!");
        }
    }

    public void EquiparArma()
    {
        temArma = true;
        balasNoCartucho = maxBalasPorCartucho;
        Debug.Log("Arma equipada! Balas: " + balasNoCartucho);

        // 🔧 Garantir que o player continue na layer correta
        gameObject.layer = LayerMask.NameToLayer("Player");

        // 🔧 E garantir que todos os filhos da arma estejam na mesma layer
        foreach (Transform child in transform)
            child.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public void AtivarUpgradeCartuchoGrande()
    {
        upgradeCartuchoGrande = true;
        upgradeTiroDuplo = false; // Desativa o outro upgrade
        maxBalasPorCartucho = 30; // Garante ou define para 30
        balasNoCartucho = Mathf.Min(balasNoCartucho, maxBalasPorCartucho); // Ajusta se necessário
        Debug.Log("Upgrade Cartucho Grande ativado! Máximo de balas por cartucho: 30");
    }

    public void AtivarUpgradeTiroDuplo()
    {
        upgradeTiroDuplo = true;
        upgradeCartuchoGrande = false; // Desativa o outro upgrade
        maxBalasPorCartucho = 30; // Volta ao padrão (ou ajuste se quiser diferente)
        Debug.Log("Upgrade Tiro Duplo ativado! Agora atira dois tiros.");
    }

    void AtualizarUI()
    {
        if (balasTexto != null)
        {
            balasTexto.text = "Balas: " + balasNoCartucho;
        }

        if (cartuchosTexto != null && ammoManager != null)
        {
            cartuchosTexto.text = "Pentes: " + ammoManager.GetCartuchos();
        }
    }
}