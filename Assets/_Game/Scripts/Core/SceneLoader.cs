using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup _loadingOverlay;
    [SerializeField] private Slider _progressBar;

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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneSequence(sceneName));
    }

    private IEnumerator LoadSceneSequence(string sceneName)
    {
        // --- BƯỚC 1: BẬT MÀN HÌNH LOADING NGAY LẬP TỨC ---
        _progressBar.value = 0;

        // Đảm bảo Alpha = 1 (Đục hoàn toàn) trước khi bật
        _loadingOverlay.alpha = 1f;
        _loadingOverlay.gameObject.SetActive(true);

        // Chờ 1 frame để đảm bảo UI kịp vẽ lên màn hình trước khi máy bị lag do load scene
        yield return null;

        // --- BƯỚC 2: LOAD SCENE NGẦM ---
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Khoan hãy hiện scene mới

        // Cập nhật thanh tiến trình
        while (operation.progress < 0.9f)
        {
            _progressBar.value = operation.progress / 0.9f;
            yield return null;
        }

        _progressBar.value = 1f;

        // Giữ màn hình loading thêm 0.5 giây cho người chơi kịp nhìn thấy 100%
        yield return new WaitForSeconds(0.5f);

        // --- BƯỚC 3: CHO PHÉP HIỆN SCENE MỚI ---
        operation.allowSceneActivation = true;

        // Đợi đến khi scene mới thực sự lên sóng
        while (!operation.isDone)
        {
            yield return null;
        }

        // --- BƯỚC 4: TẮT LOADING NGAY LẬP TỨC ---
        _loadingOverlay.gameObject.SetActive(false);
    }
}