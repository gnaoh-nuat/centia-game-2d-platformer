using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup _loadingOverlay; // Dùng CanvasGroup để chỉnh độ mờ (Alpha)
    [SerializeField] private Slider _progressBar;

    private void Awake()
    {
        // Singleton: Đảm bảo chỉ có 1 SceneLoader duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // BẤT TỬ: Không bị hủy khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Nếu lỡ tạo thêm cái thứ 2 thì hủy ngay
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneSequence(sceneName));
    }

    private IEnumerator LoadSceneSequence(string sceneName)
    {
        // 1. Bật màn hình loading (Đang trong suốt)
        _progressBar.value = 0;
        _loadingOverlay.gameObject.SetActive(true);
        _loadingOverlay.alpha = 0;

        // 2. Fade In (Dần dần tối đen)
        while (_loadingOverlay.alpha < 1)
        {
            _loadingOverlay.alpha += Time.deltaTime * 2; // Tốc độ fade
            yield return null;
        }

        // 3. Load Scene bất đồng bộ (Load ngầm)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Khoan hãy hiện scene mới vội

        while (operation.progress < 0.9f)
        {
            // Cập nhật thanh loading
            _progressBar.value = operation.progress / 0.9f;
            yield return null;
        }

        _progressBar.value = 1f;

        // Giả vờ đợi thêm 0.5s cho người chơi kịp nhìn thấy thanh full (tùy chọn)
        yield return new WaitForSeconds(0.5f);

        // 4. Cho phép hiện scene mới
        operation.allowSceneActivation = true;

        // Đợi scene mới load xong hẳn
        while (!operation.isDone)
        {
            yield return null;
        }

        // 5. Fade Out (Sáng dần lên)
        while (_loadingOverlay.alpha > 0)
        {
            _loadingOverlay.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        // Tắt màn hình loading
        _loadingOverlay.gameObject.SetActive(false);
    }
}