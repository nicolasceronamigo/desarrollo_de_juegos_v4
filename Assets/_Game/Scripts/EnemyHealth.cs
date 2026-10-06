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
    public GameObject bloodSplatPrefab;      
    public GameObject bloodParticlesPrefab;  

    [Header("Botín (Loot)")]
    public GameObject medkitPrefab; 
    [Range(0f, 100f)] 
    public float medkitDropChance = 20f; 

    [Header("Efectos de Sonido (SFX)")]
    public AudioClip deathSound; // Sonido de muerte del zombi

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

        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        if (bloodSplatPrefab != null)
        {
            Quaternion randomRot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(bloodSplatPrefab, transform.position, randomRot);
        }

        if (bloodParticlesPrefab != null) Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);

        // AUDIO DE MUERTE FORZADO Y AMPLIADO
        if (deathSound != null)
        {
            GameObject tempAudioObj = new GameObject("TempAudio_ZombieDeath");
            tempAudioObj.transform.position = transform.position;
            AudioSource aSource = tempAudioObj.AddComponent<AudioSource>();
            aSource.clip = deathSound;
            aSource.spatialBlend = 0f; // 2D para que se escuche fuerte
            aSource.volume = 2.5f;     // Forzamos volumen alto
            aSource.Play();
            Destroy(tempAudioObj, 1f); 
        }

        if (ai != null) ai.enabled = false;
        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
        if (anim != null) anim.enabled = false;

        if (medkitPrefab != null)
        {
            float randomRoll = Random.Range(0f, 100f); 
            if (randomRoll <= medkitDropChance) 
            {
                Instantiate(medkitPrefab, transform.position, Quaternion.identity);
            }
        }

        if (GameManager.Instance != null) GameManager.Instance.AddScore(10); 

        Destroy(gameObject, 0.5f);
    }
}