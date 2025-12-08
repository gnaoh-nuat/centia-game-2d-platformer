using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void OnResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    public void OnOptionsClicked()
    {
        UIManager.Instance.ShowOptionsFromPause();
    }

    public void OnQuitClicked()
    {
        GameManager.Instance.QuitToMainMenu();
    }
}