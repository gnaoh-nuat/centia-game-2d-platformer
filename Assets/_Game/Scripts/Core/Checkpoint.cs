using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _activeColor = Color.green;
    [SerializeField] private Color _inactiveColor = Color.white;

    private bool _isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated) return; // Đã kích hoạt rồi thì thôi

        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        _isActivated = true;

        // 1. Đổi màu để báo hiệu
        if (_renderer != null) _renderer.color = _activeColor;

        // 2. Lưu vị trí vào GameManager
        GameManager.Instance.UpdateCheckpoint(transform.position);

        // (Tùy chọn) Play Sound, Particle Effect...
    }

    // Hàm này để reset màu nếu muốn hệ thống chỉ cho phép 1 checkpoint sáng tại 1 thời điểm
    // (Nhưng logic đơn giản thì cứ để nó sáng mãi cũng được)
}