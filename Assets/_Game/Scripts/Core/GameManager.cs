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
        if (SceneManager.GetActiveScene().name == "Boot")
        {
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
        RespawnPosition = null;
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void HandlePauseInput()
    {
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
            Time.timeScale = 0f;
            UIManager.Instance.ShowPauseMenu();
        }
        else
        {
            Time.timeScale = 1f;
            UIManager.Instance.HidePauseMenu();
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneLoader.Instance.LoadScene("MainMenu");
        UIManager.Instance.ShowMainMenu();
    }
}