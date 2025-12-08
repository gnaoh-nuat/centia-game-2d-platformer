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
        _progressBar.value = 0;
        _loadingOverlay.alpha = 1f;
        _loadingOverlay.gameObject.SetActive(true);
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            _progressBar.value = operation.progress / 0.9f;
            yield return null;
        }

        _progressBar.value = 1f;
        yield return new WaitForSeconds(0.5f);
        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            yield return null;
        }
        _loadingOverlay.gameObject.SetActive(false);
    }
}