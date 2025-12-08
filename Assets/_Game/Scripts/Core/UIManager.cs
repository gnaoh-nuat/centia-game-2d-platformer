using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameHUD _gameHUD; // Tham chiếu tới script HUD
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
        // Mặc định khi vào game: Hiện Menu, Ẩn HUD
        ShowMainMenu();
    }

    // --- CÁC HÀM ĐIỀU KHIỂN ---

    public void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _gameHUD.gameObject.SetActive(false);
        _pauseMenuPanel.SetActive(false);
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
        // Khi Pause thì vẫn hiện HUD ở dưới, hoặc ẩn đi tùy bạn
    }

    // --- CẦU NỐI (BRIDGE) ---
    // Hàm này để Player khi sinh ra sẽ gọi: "Ê UI, tôi đây, hiển thị máu cho tôi đi"
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
        // Tùy chọn: Có thể ẩn HUD đi cho thoáng, hoặc để mờ mờ ở dưới
        // _gameHUD.gameObject.SetActive(false); 
    }

    public void HidePauseMenu()
    {
        _pauseMenuPanel.SetActive(false);
        _optionsPanel.SetActive(false); // Đảm bảo tắt cả Option nếu đang mở
        // _gameHUD.gameObject.SetActive(true);
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