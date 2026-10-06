using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Interfaz (UI)")]
    public Slider healthSlider;
    public CanvasGroup healthUIGroup; 
    public float showDuration = 3f;   
    private float hideTimer;            

    [Header("Daño y Colisiones")]
    public float damageFromZombies = 20f;
    public float invulnerabilityTime = 1f;
    private float lastDamageTime = -1f;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthUIGroup != null)
        {
            healthUIGroup.alpha = 0f;
        }
    }

    void Update()
    {
        if (healthUIGroup != null && healthUIGroup.alpha > 0)
        {
            hideTimer -= Time.deltaTime;

            if (hideTimer <= 0)
            {
                healthUIGroup.alpha -= Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthUIGroup != null)
        {
            healthUIGroup.alpha = 1f;
            hideTimer = showDuration;
        }

        Debug.Log("¡El jugador recibió daño! Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        
        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthUIGroup != null)
        {
            healthUIGroup.alpha = 1f;
            hideTimer = showDuration;
        }

        Debug.Log("¡Jugador curado! Vida actual: " + currentHealth);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Time.time >= lastDamageTime + invulnerabilityTime)
            {
                TakeDamage(damageFromZombies);
                lastDamageTime = Time.time;
            }
        }
    }

    private void Die()
    {
        currentHealth = 0;
        
        // CORREGIDO: Usamos FindAnyObjectByType para evitar la advertencia en Unity
        GameOverManager gameOver = FindAnyObjectByType<GameOverManager>();
        if (gameOver != null)
        {
            gameOver.MostrarGameOver();
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.Play();
        }

        gameObject.SetActive(false);
    }
}