using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackForce = 10f;

    // Xử lý khi va chạm với Trigger (Ví dụ: Chông gai, Đạn xuyên)
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other.gameObject);
    }

    // Xử lý khi va chạm vật lý (Ví dụ: Va vào người quái vật)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void TryDealDamage(GameObject target)
    {
        // 1. Kiểm tra Tag trước tiên (Tối ưu hiệu năng)
        if (!target.CompareTag("Player")) return;

        // 2. Kiểm tra xem đối tượng có nhận sát thương được không
        if (target.TryGetComponent(out IDamageable damageable))
        {
            // Tính hướng đẩy lùi (Từ tâm vật gây sát thương -> Hướng về Player)
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // Tinh chỉnh hướng đẩy (Optional):
            // Nếu va chạm chủ yếu theo chiều dọc (đạp lên đầu hoặc bị húc từ dưới),
            // ta bẻ lái lực đẩy sang hai bên một chút để nhân vật không bị nảy thẳng đứng mãi.
            if (Mathf.Abs(direction.x) < 0.5f)
            {
                // Nếu player ở bên phải -> đẩy phải (1), ngược lại đẩy trái (-1)
                direction.x = target.transform.position.x > transform.position.x ? 1f : -1f;
            }

            // Luôn đảm bảo có chút lực đẩy lên trên để nhân vật nảy khỏi mặt đất (tránh ma sát sàn)
            if (direction.y < 0.2f) direction.y = 0.2f;

            // 3. Gây sát thương và đẩy lùi
            damageable.TakeDamage(_damageAmount, direction.normalized * _knockbackForce);
        }
    }
}