using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class GameOverManager : MonoBehaviour
{
    [Header("UI de Game Over")]
    public GameObject gameOverPanel; // El panel que aparecerá al morir

    void Start()
    {
        // Nos aseguramos de que el panel esté oculto al iniciar el juego
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Esta función se activará cuando el jugador muera
    public void MostrarGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Opcional: Pausa el tiempo del juego para que los zombis se detengan por completo
        Time.timeScale = 0f;
    }

    // Botón para Jugar de nuevo / Reiniciar nivel
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f; // Restauramos el tiempo normal antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botón para volver al Menú Principal (Asegúrate de que tu escena de menú se llame "MainMenu" o cámbialo por su nombre exacto)
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}