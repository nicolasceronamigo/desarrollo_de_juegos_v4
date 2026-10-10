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
                // 1. Asumimos el daño normal por defecto
                float damageToTake = damageFromZombies; 

                // 2. Leemos las estadísticas de este enemigo en específico
                EnemyHealth enemyStats = collision.gameObject.GetComponent<EnemyHealth>();
                
                // 3. Si tiene el script EnemyHealth, usamos su daño personalizado
                if (enemyStats != null)
                {
                    damageToTake = enemyStats.damageToPlayer; 
                }

                TakeDamage(damageToTake);
                lastDamageTime = Time.time;
            }
        }
    }

    private void Die()
    {
        currentHealth = 0;
        
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