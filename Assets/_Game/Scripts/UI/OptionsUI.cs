using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public void OnBackClicked()
    {
        // Kiểm tra xem game có đang Pause không
        if (GameManager.Instance.IsPaused)
        {
            // Đang Pause -> Về lại Pause Menu
            UIManager.Instance.ReturnToPauseFromOptions();
        }
        else
        {
            // Không Pause -> Về lại Main Menu
            UIManager.Instance.ShowMainMenu(); // Bạn cần sửa hàm này trong UIManager một chút để nó tắt Options đi
        }
    }
}