
using UnityEngine;

public class MejoraDano : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player"))
            return;

        PlayerController jugador = otro.GetComponentInParent<PlayerController>();

        if (jugador == null)
            jugador = otro.GetComponentInChildren<PlayerController>();

        if (jugador == null)
        {
            Debug.LogWarning("No se encontró PlayerController.");
            return;
        }

        jugador.MejorarDano();

        Destroy(gameObject);
    }
}
