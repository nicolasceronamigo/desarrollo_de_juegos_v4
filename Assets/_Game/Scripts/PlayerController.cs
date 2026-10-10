// PlayerController.cs

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f; // Velocidad de la ráfaga
    [Header("Mejora de daño")]
    [SerializeField] private float danoBase = 10f;
    [SerializeField] private float aumentoPorMejora = 2f;
    [SerializeField] private int nivelMejora = 0;

    public float DanoActual
    {
        get { return danoBase + nivelMejora * aumentoPorMejora; }
    }
    private float nextFireTime = 0f;

    [Header("Efectos de Sonido (SFX)")]
    public AudioClip shootSound;   // Aquí arrastrarás tu audio de disparo
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 moveInput;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        
        // Obtenemos el componente AudioSource automáticamente del jugador
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no lo tiene puesto, se lo agregamos por código de seguridad
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0f; // Sonido 2D
        }
    }

    void Update()
    {
        // 1. WASD
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. Ratón
        if (mainCam != null)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        // 3. Disparo con Clic Izquierdo continuo
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;

        Vector2 lookDir = mousePos - rb.position;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject nuevaBala = Instantiate(
                bulletPrefab,
                firePoint.position,
                firePoint.rotation
            );

            Bullet bullet = nuevaBala.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.ConfigurarDano(DanoActual);
            }
        }

        // REPRODUCIR EL SONIDO DE DISPARO
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    public void MejorarDano()
    {
        nivelMejora++;

        Debug.Log(
            "Nivel de mejora: " + nivelMejora +
            " | Daño actual: " + DanoActual
        );
    }
}