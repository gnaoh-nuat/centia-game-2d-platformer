using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InputReader _inputReader;
    public bool IsPaused { get; private set; } = false;

    public Vector2? RespawnPosition { get; private set; }

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

    private void OnEnable()
    {
        _inputReader.PauseEvent += HandlePauseInput;
    }

    private void OnDisable()
    {
        _inputReader.PauseEvent -= HandlePauseInput;
    }

    private void Start()
    {
        // Nếu game bắt đầu từ Scene "Boot", hãy tự động chuyển sang MainMenu
        if (SceneManager.GetActiveScene().name == "Boot")
        {
            // Gọi SceneLoader để chuyển cảnh
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }

    public void UpdateCheckpoint(Vector2 position)
    {
        RespawnPosition = position;
        Debug.Log($"Checkpoint Updated: {position}");
    }

    public void ClearCheckpoint()
    {
        RespawnPosition = null; // Dùng khi New Game
    }

    // Hàm thoát game
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void HandlePauseInput()
    {
        // Chỉ cho phép Pause khi đang ở trong Gameplay (Không phải ở MainMenu)
        // Cách đơn giản nhất check: Nếu HUD đang hiện thì tức là đang chơi
        if (UIManager.Instance.IsHUDActive())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f; // Dừng thời gian
            UIManager.Instance.ShowPauseMenu(); // Hiện UI
        }
        else
        {
            Time.timeScale = 1f; // Chạy tiếp
            UIManager.Instance.HidePauseMenu(); // Ẩn UI
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // QUAN TRỌNG: Phải trả lại thời gian trước khi load scene
        IsPaused = false;

        // Load về Menu
        SceneLoader.Instance.LoadScene("MainMenu");

        // Bảo UIManager bật lại giao diện Menu
        UIManager.Instance.ShowMainMenu();
    }
}