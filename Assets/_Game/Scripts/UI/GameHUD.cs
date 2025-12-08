using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameHUD : MonoBehaviour
{
    // Bỏ SerializeField của Player đi vì ta sẽ gán bằng code từ UIManager
    private PlayerController _player;

    [Header("UI Elements")]
    [SerializeField] private Image _staminaFillImage;
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private GameObject _heartPrefab;

    // List chứa các trái tim đang hiển thị
    private List<GameObject> _hearts = new List<GameObject>();

    // --- HÀM KHỞI TẠO (Được UIManager gọi khi Player sinh ra) ---
    public void InitializeHUD(PlayerController player)
    {
        // 1. Hủy đăng ký player cũ (nếu có - trường hợp respawn)
        if (_player != null)
        {
            _player.Health.OnHealthChanged -= UpdateHealthUI;
            _player.Stamina.OnStaminaChanged -= UpdateStaminaUI;
        }

        // 2. Gán player mới
        _player = player;

        // 3. Setup lại giao diện ngay lập tức
        if (_player != null)
        {
            InitializeHearts(_player.Health.MaxHealth);
            UpdateHealthUI(_player.Health.CurrentHealth, _player.Health.MaxHealth);
            UpdateStaminaUI(_player.Stamina.CurrentStamina, _player.Stats.MaxStamina);

            // 4. Đăng ký sự kiện mới để lắng nghe thay đổi
            _player.Health.OnHealthChanged += UpdateHealthUI;
            _player.Stamina.OnStaminaChanged += UpdateStaminaUI;
        }
    }

    private void OnDestroy()
    {
        // Dọn dẹp khi tắt game
        if (_player != null)
        {
            _player.Health.OnHealthChanged -= UpdateHealthUI;
            _player.Stamina.OnStaminaChanged -= UpdateStaminaUI;
        }
    }

    // --- CÁC HÀM LOGIC HIỂN THỊ (PHẦN BẠN BỊ THIẾU) ---

    private void InitializeHearts(int maxHealth)
    {
        // Xóa hết tim cũ
        foreach (Transform child in _heartContainer) Destroy(child.gameObject);
        _hearts.Clear();

        // Sinh ra tim mới
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(_heartPrefab, _heartContainer);
            _hearts.Add(newHeart);
        }
    }

    private void UpdateHealthUI(int current, int max)
    {
        // Đảm bảo số lượng tim khớp với MaxHealth (phòng trường hợp tăng Max HP khi chơi)
        if (_hearts.Count != max) InitializeHearts(max);

        // Bật/Tắt tim dựa trên máu hiện tại
        for (int i = 0; i < _hearts.Count; i++)
        {
            // Nếu index nhỏ hơn máu hiện tại -> Bật (Tim đỏ)
            // Nếu index lớn hơn hoặc bằng -> Tắt (Mất máu)
            _hearts[i].SetActive(i < current);
        }
    }

    private void UpdateStaminaUI(float current, float max)
    {
        if (_staminaFillImage == null) return;

        // Tính tỉ lệ (0 đến 1)
        float ratio = current / max;

        // Thay đổi Scale X của thanh Stamina (Vì bạn dùng Image Type: Simple)
        _staminaFillImage.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}