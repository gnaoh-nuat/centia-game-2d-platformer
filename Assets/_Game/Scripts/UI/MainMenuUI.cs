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
        // 1. Setup giá trị ban đầu cho Options UI từ cài đặt hiện tại
        _fullscreenToggle.isOn = Screen.fullScreen;

        // Lấy volume hiện tại từ Mixer để gán vào slider (chuyển đổi ngược từ dB về Linear 0-1)
        float currentMusicVol;
        _audioMixer.GetFloat("MusicVol", out currentMusicVol);
        _musicSlider.value = Mathf.Pow(10, currentMusicVol / 20);

        float currentSFXVol;
        _audioMixer.GetFloat("SFXVol", out currentSFXVol);
        _sfxSlider.value = Mathf.Pow(10, currentSFXVol / 20);

        // 2. Gán sự kiện (Listener) cho các UI Element
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    // --- BUTTON EVENTS (Gắn vào OnClick trong Inspector) ---

    public void OnPlayClicked()
    {
        // Xóa dữ liệu cũ (để chắc chắn là chơi mới hoàn toàn)
        PlayerPrefs.DeleteKey("Save_Scene");

        // Gọi SceneLoader để vào màn 1
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

    // --- OPTIONS LOGIC ---

    public void SetMusicVolume(float value)
    {
        // Chuyển đổi từ thanh trượt (0.0001 -> 1) sang Decibel (-80 -> 0)
        // Dùng Mathf.Log10 để chỉnh âm lượng tự nhiên hơn
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