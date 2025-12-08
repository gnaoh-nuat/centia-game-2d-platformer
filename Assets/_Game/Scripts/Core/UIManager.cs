using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _optionsPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _gameHUD.gameObject.SetActive(false);
        _pauseMenuPanel.SetActive(false);
        _optionsPanel.SetActive(false);
    }

    public void ShowGameplayHUD()
    {
        _mainMenuPanel.SetActive(false);
        _gameHUD.gameObject.SetActive(true);
        _pauseMenuPanel.SetActive(false);
    }

    public void TogglePauseMenu(bool isPaused)
    {
        _pauseMenuPanel.SetActive(isPaused);
    }

    public void BindPlayerToUI(PlayerController player)
    {
        _gameHUD.InitializeHUD(player);
    }

    public bool IsHUDActive()
    {
        return _gameHUD.gameObject.activeSelf;
    }

    public void ShowPauseMenu()
    {
        _pauseMenuPanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _pauseMenuPanel.SetActive(false);
        _optionsPanel.SetActive(false);
    }

    public void ShowOptionsFromPause()
    {
        _pauseMenuPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    public void ReturnToPauseFromOptions()
    {
        _optionsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }
}