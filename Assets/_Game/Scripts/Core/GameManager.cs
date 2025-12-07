using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        // Nếu game bắt đầu từ Scene "Boot", hãy tự động chuyển sang MainMenu
        if (SceneManager.GetActiveScene().name == "Boot")
        {
            // Gọi SceneLoader để chuyển cảnh
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }

    // Hàm thoát game
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}