using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        LoadSettings();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(
            "MasterVolume",
            Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20
        );

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        Debug.Log("Volumen guardado: " + volume);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;

        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Aplicar volumen
        audioMixer.SetFloat(
            "MasterVolume",
            Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20
        );

        // Aplicar pantalla completa
        Screen.fullScreen = fullscreen;

        // Actualizar controles
        volumeSlider.SetValueWithoutNotify(volume);
        fullscreenToggle.SetIsOnWithoutNotify(fullscreen);
    }
}