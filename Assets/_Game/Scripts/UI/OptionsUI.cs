using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public void OnBackClicked()
    {
        if (GameManager.Instance.IsPaused)
        {
            UIManager.Instance.ReturnToPauseFromOptions();
        }
        else
        {
            UIManager.Instance.ShowMainMenu();
        }
    }
}