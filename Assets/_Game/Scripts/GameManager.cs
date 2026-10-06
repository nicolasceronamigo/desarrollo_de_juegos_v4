using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class GameManager : MonoBehaviour
{
    // Esto es un Singleton: nos permite acceder al GameManager desde cualquier otro script fácilmente
    public static GameManager Instance; 

    [Header("Puntuación")]
    public int score = 0;
    public TextMeshProUGUI scoreText; // Aquí arrastraremos tu texto de la UI

    void Awake()
    {
        // Configuramos el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarTextoScore();
    }

    // Esta función la llamarán los zombies al morir
    public void AddScore(int points)
    {
        score += points;
        ActualizarTextoScore();
    }

    void ActualizarTextoScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}