using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;

    [Header("Options")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _fullscreenToggle;

    private void Start()
    {
        _fullscreenToggle.isOn = Screen.fullScreen;

        float currentMusicVol;
        _audioMixer.GetFloat("MusicVol", out currentMusicVol);
        _musicSlider.value = Mathf.Pow(10, currentMusicVol / 20);

        float currentSFXVol;
        _audioMixer.GetFloat("SFXVol", out currentSFXVol);
        _sfxSlider.value = Mathf.Pow(10, currentSFXVol / 20);

        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void OnPlayClicked()
    {
        PlayerPrefs.DeleteKey("Save_Scene");

        SceneLoader.Instance.LoadScene("Level_01");

        UIManager.Instance.ShowGameplayHUD();
    }

    public void OnOptionsClicked()
    {
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    public void OnBackClicked()
    {
        _optionsPanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        GameManager.Instance.ExitGame();
    }

    public void SetMusicVolume(float value)
    {
        float dbVolume = Mathf.Log10(value) * 20;
        _audioMixer.SetFloat("MusicVol", dbVolume);
    }

    public void SetSFXVolume(float value)
    {
        float dbVolume = Mathf.Log10(value) * 20;
        _audioMixer.SetFloat("SFXVol", dbVolume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}