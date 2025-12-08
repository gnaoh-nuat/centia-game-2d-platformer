using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameHUD _gameHUD; // Tham chiếu tới script HUD
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _loadingScreen;

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
}