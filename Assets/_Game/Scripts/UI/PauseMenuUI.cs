using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void OnResumeClicked()
    {
        // Gọi lại hàm Toggle để nó tự resume
        GameManager.Instance.TogglePause();
    }

    public void OnOptionsClicked()
    {
        // Nhờ UIManager chuyển sang bảng Option
        UIManager.Instance.ShowOptionsFromPause();
    }

    public void OnQuitClicked()
    {
        // Gọi GameManager xử lý việc thoát
        GameManager.Instance.QuitToMainMenu();
    }
}