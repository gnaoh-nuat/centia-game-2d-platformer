using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameHUD : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private PlayerController _player; // Tham chiếu đến Player để lấy hệ thống con

    [Header("Stamina UI")]
    [SerializeField] private Image _staminaFillImage;

    [Header("Health UI")]
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private GameObject _heartPrefab; // Prefab trái tim đỏ (placeholder)

    // List chứa các trái tim đang hiển thị
    private List<GameObject> _hearts = new List<GameObject>();

    private void Start()
    {
        // Khởi tạo UI lần đầu
        if (_player != null)
        {
            // Setup Máu
            InitializeHearts(_player.Health.MaxHealth);
            UpdateHealthUI(_player.Health.CurrentHealth, _player.Health.MaxHealth);

            // Setup Stamina
            UpdateStaminaUI(_player.Stamina.CurrentStamina, _player.Stats.MaxStamina);

            // Đăng ký sự kiện
            _player.Health.OnHealthChanged += UpdateHealthUI;
            _player.Stamina.OnStaminaChanged += UpdateStaminaUI;
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký khi chuyển cảnh hoặc tắt game
        if (_player != null)
        {
            if (_player.Health != null)
                _player.Health.OnHealthChanged -= UpdateHealthUI;

            if (_player.Stamina != null)
                _player.Stamina.OnStaminaChanged -= UpdateStaminaUI;
        }
    }

    // --- LOGIC MÁU ---

    private void InitializeHearts(int maxHealth)
    {
        // Xóa hết tim cũ (nếu có)
        foreach (Transform child in _heartContainer) Destroy(child.gameObject);
        _hearts.Clear();

        // Sinh ra tim mới
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(_heartPrefab, _heartContainer);
            _hearts.Add(newHeart);
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Đảm bảo số lượng tim khớp với MaxHealth (phòng trường hợp tăng Max HP)
        if (_hearts.Count != maxHealth) InitializeHearts(maxHealth);

        // Bật/Tắt tim dựa trên máu hiện tại
        for (int i = 0; i < _hearts.Count; i++)
        {
            // Nếu index nhỏ hơn máu hiện tại -> Bật (Tim đỏ)
            // Nếu index lớn hơn hoặc bằng -> Tắt (Mất máu)
            // Ví dụ: Máu = 3. Index 0, 1, 2 sẽ true. Index 3, 4 sẽ false.
            _hearts[i].SetActive(i < currentHealth);

            // *Mẹo nâng cao*: Nếu có Asset "Tim rỗng" (Empty Heart), thay vì SetActive(false), 
            // bạn có thể đổi Sprite của nó sang hình trái tim vỡ/đen.
        }
    }

    // --- LOGIC STAMINA ---

    private void UpdateStaminaUI(float current, float max)
    {
        // Tính tỉ lệ phần trăm (0 đến 1)
        float fillAmount = current / max;
        _staminaFillImage.fillAmount = fillAmount;
    }
}