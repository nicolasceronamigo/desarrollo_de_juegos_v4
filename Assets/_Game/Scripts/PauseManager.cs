using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsPanel;

    private bool isPaused = false;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;

        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;

        pauseMenu.SetActive(false);
        optionsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenOptions()
    {
        pauseMenu.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}