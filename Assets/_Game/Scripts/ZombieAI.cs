using UnityEngine;
using UnityEngine.AI; // Necesario para NavMesh

public class ZombieAI : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2.5f;
    public float detectionRange = 12f;
    public float stoppingDistance = 1.1f;

    [Header("Rotación y Orientación")]
    [Tooltip("0 = mira a la derecha, -90 = mira hacia arriba")]
    public float rotationOffset = 0f;

    [Header("Animación Procedural")]
    public float wobbleSpeed = 12f;
    public float wobbleAngle = 10f;

    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Configuración esencial para juegos 2D:
        agent.updateRotation = false; // Evita que Unity intente rotar el zombi en 3D
        agent.updateUpAxis = false;   // Mantiene al zombi en el plano 2D (XY)

        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;

        BuscarJugador();
    }

    void Update()
    {
        // Si el jugador no existe o está desactivado
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            BuscarJugador();
            if (agent.isOnNavMesh) agent.isStopped = true;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Si está dentro del rango de detección
        if (distance <= detectionRange)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position); // ¡NavMesh calcula el camino solo!
            }

            ControlarRotacionYAnimacion();
        }
        else
        {
            if (agent.isOnNavMesh) agent.isStopped = true;
        }
    }

    private void ControlarRotacionYAnimacion()
    {
        // Si el zombi se está moviendo, miramos hacia la dirección a la que camina
        Vector2 direccionMovimiento = agent.velocity;

        if (direccionMovimiento.sqrMagnitude > 0.05f)
        {
            float baseAngle = Mathf.Atan2(direccionMovimiento.y, direccionMovimiento.x) * Mathf.Rad2Deg + rotationOffset;
            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAngle;
            transform.rotation = Quaternion.Euler(0, 0, baseAngle + wobble);
        }
        else
        {
            // Si está detenido al lado del jugador, mira directo hacia él sin tambaleo
            Vector2 direccionJugador = (player.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(direccionJugador.y, direccionJugador.x) * Mathf.Rad2Deg + rotationOffset;
            transform.rotation = Quaternion.Euler(0, 0, baseAngle);
        }
    }

    void BuscarJugador()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target != null && target.activeInHierarchy)
        {
            player = target.transform;
        }
        else
        {
            player = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}