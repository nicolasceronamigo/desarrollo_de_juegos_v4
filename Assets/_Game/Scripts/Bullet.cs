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
        // Ignorar si choca con el propio jugador
        if (hitInfo.CompareTag("Player")) return;

        // Comprobar si golpeó a un enemigo con vida
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destruir la bala al chocar
        Destroy(gameObject);
    }
}