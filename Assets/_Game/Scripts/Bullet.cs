using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 45f;
    public float lifeTime = 2f;
    public float damage = 10f; // Cada bala quita 10

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Ignorar si choca con el jugador
        if (hitInfo.CompareTag("Player")) return;

        // 2. NUEVO: Ignorar si choca con el botiquín (lo atraviesa)
        if (hitInfo.GetComponent<HealthPickup>() != null) return;

        // 3. Comprobar si golpeó a un enemigo
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // 4. Destruir la bala al chocar (contra enemigos o paredes)
        Destroy(gameObject);
    }
}