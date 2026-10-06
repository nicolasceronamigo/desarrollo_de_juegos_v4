using UnityEngine;

public class Puerta : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private bool estaAbierta = false;

    public bool EstaAbierta => estaAbierta;

    [SerializeField] private float anguloPuertaAbierta = 90f;

    private Quaternion rotacionCerrada;

    [SerializeField] private float distanciaInteraccion = 1.5f;
    [SerializeField] private Transform puntoInteraccion;
    private Transform player;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rotacionCerrada = transform.rotation;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            player = jugador.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player != null)
        {
            float distancia = Vector2.Distance(puntoInteraccion.position, player.position);

            if (distancia <= distanciaInteraccion)
            {
                AlternarPuerta();
            }
        }
    }

    private void AlternarPuerta()
    {
        estaAbierta = !estaAbierta;

        boxCollider.enabled = !estaAbierta;

        if (estaAbierta)
        {
            transform.rotation = rotacionCerrada * Quaternion.Euler(0, 0, anguloPuertaAbierta);

            Debug.Log("Puerta abierta");
        }
        else
        {
            transform.rotation = rotacionCerrada;

            Debug.Log("Puerta cerrada");
        }
    }
}