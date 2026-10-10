using UnityEngine;

public class PuertaVida : MonoBehaviour
{
    [Header("Vida de la puerta")]
    public float vidaMaxima = 100f;

    private float vidaActual;
    private Puerta puerta;

    [Header("Sonido de destrucción")]
    [SerializeField] private AudioClip sonidoDestruccion;
    [SerializeField] private float volumenDestruccion = 1f;

    [Header("Sonido de daño")]
    [SerializeField] private AudioClip sonidoDanio;
    [SerializeField] private float volumenDanio = 1f;

private AudioSource audioDanio;

    [Header("Daño por zombies")]
    [SerializeField] private float distanciaDeteccion = 1.5f;
    [SerializeField] private int zombiesNecesarios = 2;
    [SerializeField] private float danoPorSegundo = 10f;

    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private Transform puntoDeteccionZombies;

    private int ContarZombiesCerca()
    {
        Collider2D[] zombies = Physics2D.OverlapCircleAll(
            puntoDeteccionZombies.position,
            distanciaDeteccion,
            zombieLayer
        );

        return zombies.Length;
    }

    void Update()
    {
        if (puerta.EstaAbierta)
            return;

        int cantidadZombies = ContarZombiesCerca();

        if (cantidadZombies >= zombiesNecesarios)
        {
            RecibirDanio(danoPorSegundo * Time.deltaTime);

            if (audioDanio != null && sonidoDanio != null && !audioDanio.isPlaying)
            {
                audioDanio.Play();
            }
        }
        else
        {
            if (audioDanio != null && audioDanio.isPlaying)
            {
                audioDanio.Stop();
            }
        }
    }

    void Start()
    {
        vidaActual = vidaMaxima;
        puerta = GetComponent<Puerta>();
        audioDanio = gameObject.AddComponent<AudioSource>();
        audioDanio.clip = sonidoDanio;
        audioDanio.volume = volumenDanio;
        audioDanio.spatialBlend = 1f;
        audioDanio.rolloffMode = AudioRolloffMode.Linear;
        audioDanio.minDistance = 1f;
        audioDanio.maxDistance = 75f;
        audioDanio.loop = true;
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;

        Debug.Log("La puerta recibió " + cantidad + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            DestruirPuerta();
        }
    }

    private void DestruirPuerta()
    {
        Debug.Log("¡La puerta fue destruida!");

        if (sonidoDestruccion != null)
        {
            GameObject objetoSonido = new GameObject("SonidoDestruccionPuerta");

            objetoSonido.transform.position = transform.position;

            AudioSource audioSource = objetoSonido.AddComponent<AudioSource>();

            audioSource.clip = sonidoDestruccion;
            audioSource.volume = volumenDestruccion;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 75f;
            audioSource.Play();

            Destroy(objetoSonido, sonidoDestruccion.length);
        }

        Destroy(gameObject);
    }
}