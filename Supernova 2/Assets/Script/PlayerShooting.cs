using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; // onde o tiro nasce (alinhado com a arma/c�mera)
    public Camera mainCamera;
    public bool temArma = false;

    [Header("Muni��o")]
    public int maxBalasPorCartucho = 30;
    public int balasNoCartucho = 30;

    public TextMeshProUGUI balasTexto;
    public TextMeshProUGUI cartuchosTexto;

    private AmmoManager ammoManager;

    void Start()
    {
        ammoManager = GetComponent<AmmoManager>()
                   ?? GetComponentInParent<AmmoManager>()
                   ?? FindObjectOfType<AmmoManager>();

        if (ammoManager == null)
            Debug.LogWarning("PlayerShooting: nenhum AmmoManager encontrado. Pickup pode n�o funcionar.");

        if (temArma) balasNoCartucho = maxBalasPorCartucho;
        AtualizarUI();

        // Verifica��o de setup (adicionei para debug)
        if (mainCamera == null) Debug.LogError("PlayerShooting: Atribua a mainCamera no Inspector!");
        if (firePoint == null) Debug.LogError("PlayerShooting: Atribua o firePoint no Inspector!");
    }

    void Update()
    {
#if !UNITY_ANDROID && !UNITY_IOS
        if (temArma && Input.GetMouseButtonDown(0)) TentarAtirar();
        if (temArma && Input.GetKeyDown(KeyCode.R)) Recarregar();
#endif
        AtualizarUI();
    }

    public void TentarAtirar()
    {
        if (balasNoCartucho > 0)
        {
            Shoot();
            balasNoCartucho--;
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

    void Shoot()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Shoot: mainCamera n�o atribu�da!");
            return;
        }

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro da tela

        // Sempre use a dire��o do ray (infinita). Opcional: limite dist�ncia para simular "alcance"
        Vector3 direction = ray.direction;
        float shootSpeed = 40f; // Velocidade da bala
        float spawnOffset = 0.5f; // Spawn ligeiramente � frente para evitar colis�o inicial com player

        // Posi��o de spawn: firePoint + um pouco na dire��o para evitar overlap
        Vector3 spawnPos = firePoint.position + direction * spawnOffset;

        // Instancie o proj�til
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));

        // Ignore colis�o com o player (evita sumi�o imediato)
        Collider projCollider = proj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>(); // Assumindo que o player tem Collider
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
            Debug.LogError("Proj�til sem Rigidbody! Adicione um no prefab.");
        }

        // Debug para testar dire��o
        Debug.Log($"Tiro disparado! Dire��o: {direction}, Posi��o: {spawnPos}");
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
            balasNoCartucho = maxBalasPorCartucho;
        }
        else
        {
            Debug.Log("Sem cartuchos sobrando!");
        }
    }

    public void EquiparArma()
    {
        temArma = true;
        balasNoCartucho = maxBalasPorCartucho; // Adicionei: defina balas ao equipar (caso n�o esteja em Start)
        Debug.Log("Arma equipada! Balas: " + balasNoCartucho);
    }

    void AtualizarUI()
    {
        if (balasTexto != null)
        {
            balasTexto.text = "Balas: " + balasNoCartucho;
        }

        if (cartuchosTexto != null && ammoManager != null)
        {
            cartuchosTexto.text = "Cartuchos: " + ammoManager.GetCartuchos();
        }
    }
}