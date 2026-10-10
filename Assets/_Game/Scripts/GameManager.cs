using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    [Header("Puntuación")]
    public int score = 0;
    public TextMeshProUGUI scoreText; 

    [Header("Fase Final (Jefe)")]
    public int puntosParaJefe = 10;    // Dejado en 10 para probar rápido
    public GameObject bossPrefab;      
    public Transform bossSpawnPoint;   
    private bool bossSpawned = false;  

    [Header("UI Victoria")]
    public GameObject victoryPanel;    // Aquí debes arrastrar tu texto "¡HAS SOBREVIVIDO!"

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ActualizarTextoScore();
        
        // Nos aseguramos de que el panel de victoria empiece oculto
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        ActualizarTextoScore();

        if (score >= puntosParaJefe && !bossSpawned)
        {
            ActivarFaseJefe();
        }
    }

    void ActualizarTextoScore()
    {
        if (scoreText != null) scoreText.text = "Puntos: " + score;
    }

    private void ActivarFaseJefe()
    {
        bossSpawned = true;
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        }
    }

    public void MostrarVictoria()
    {
        // 1. Activa el cartel en pantalla
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // 2. Congela el tiempo para terminar el juego
        Time.timeScale = 0f; 
        Debug.Log("¡Jefe derrotado! El jugador gana y el juego se detiene.");
    }
}