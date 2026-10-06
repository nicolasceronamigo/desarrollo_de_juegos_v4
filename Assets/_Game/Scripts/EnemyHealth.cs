using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float maxHealth = 100f;
    [Tooltip("Si la vida máxima es menor o igual a este valor, no mostrará barra flotante")]
    public float minHealthToShowBar = 30f;
    private float currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthSlider;
    public Image fillImage;

    [Header("Colores de Barra")]
    public Color fullHealthColor = Color.green;
    public Color mediumHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    [Header("Efectos de Muerte (Gore SAS)")]
    public GameObject bloodSplatPrefab;      // Charco de sangre para el suelo
    public GameObject bloodParticlesPrefab;  // Explosión de partículas (BloodExplosion)

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private ZombieAI ai;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        ai = GetComponent<ZombieAI>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthSlider.gameObject.SetActive(false);
        }

        UpdateHealthVisuals();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthSlider != null && maxHealth > minHealthToShowBar)
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = currentHealth;
            UpdateHealthVisuals();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthVisuals()
    {
        if (fillImage == null) return;

        float healthRatio = currentHealth / maxHealth;

        if (healthRatio > 0.5f)
        {
            fillImage.color = Color.Lerp(mediumHealthColor, fullHealthColor, (healthRatio - 0.5f) * 2f);
        }
        else
        {
            fillImage.color = Color.Lerp(lowHealthColor, mediumHealthColor, healthRatio * 2f);
        }
    }

    void Die()
    {
        isDead = true;

        // 1. Ocultar la barra de vida de inmediato
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }

        // 2. Spawnear charco de sangre en el suelo con rotación aleatoria (360°)
        if (bloodSplatPrefab != null)
        {
            Quaternion randomRot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(bloodSplatPrefab, transform.position, randomRot);
        }

        // 3. Spawnear explosión de partículas de sangre
        if (bloodParticlesPrefab != null)
        {
            Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
        }

        // 4. Apagar IA, colisiones y físicas
        if (ai != null) ai.enabled = false;
        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;

        // 5. Ocultar el sprite del cuerpo de inmediato para simular el estallido
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        // Desactivar Animator para cortar la animación de muerte defectuosa
        if (anim != null) anim.enabled = false;

        // 6. Eliminar el objeto de la escena tras procesar los efectos
        Destroy(gameObject, 0.5f);
    }
}