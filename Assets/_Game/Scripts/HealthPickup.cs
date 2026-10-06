using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Configuración")]
    public float healAmount = 25f; // Cuánta vida recupera
    public float lifeTime = 10f;   // Tiempo en segundos antes de desaparecer del suelo

    void Start()
    {
        Destroy(gameObject, lifeTime); // Se autodestruye si no lo recogen
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador entra en contacto con este objeto...
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // Manda a llamar la función que creamos en el Paso 1
                Destroy(gameObject);           // Destruye el botiquín de la pantalla
            }
        }
    }
}