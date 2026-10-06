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

    // ====== ZONA DE MUERTE: GAME OVER Y DESAPARICIÓN ======
    private void Die()
    {
        currentHealth = 0;
        Debug.Log("El jugador ha muerto. Activando menú de Game Over.");

        // 1. Activamos el menú de Game Over a través del gestor en la escena
        GameOverManager gameOver = FindFirstObjectByType<GameOverManager>();
        if (gameOver != null)
        {
            gameOver.MostrarGameOver();
        }

        // 2. Reproducir sonido de muerte antes de ocultar el objeto (si tiene un AudioSource)
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.Play();
        }

        // 3. Desactivamos por completo el GameObject del jugador para que desaparezca
        gameObject.SetActive(false);
    }
}