using UnityEngine;

public class WindArea : MonoBehaviour
{
    [Header("Cấu hình Gió")]
    [Tooltip("Hướng gió thổi (X, Y). Ví dụ: (1, 0) là sang phải, (0, 1) là lên trời.")]
    public Vector2 direction = Vector2.right;

    [Tooltip("Độ mạnh của gió")]
    public float strength = 20f;

    [Header("Cấu hình Mục tiêu")]
    [Tooltip("Tên Tag của nhân vật (mặc định là Player)")]
    public string targetTag = "Player";

    // Hàm này chạy liên tục mỗi frame khi có vật thể nằm trong vùng Trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        // 1. Chỉ tác động nếu vật thể có Tag là "Player"
        if (other.CompareTag(targetTag))
        {
            // 2. Lấy Rigidbody2D của Player
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 3. Tính toán vector lực: Hướng (đã chuẩn hóa) * Độ mạnh * Time.deltaTime
                // Dùng Time.deltaTime để lực tác động mượt mà theo thời gian thực
                Vector2 force = direction.normalized * strength * Time.deltaTime * 50f; // Nhân 50f để số nhập vào Inspector không cần quá lớn

                // 4. Cộng lực vào Player
                rb.AddForce(force);
            }
        }
    }

    // --- PHẦN VẼ DEBUG ĐỂ DỄ NHÌN TRONG EDITOR ---
    private void OnDrawGizmos()
    {
        // Vẽ màu xanh mờ thể hiện vùng gió
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);

        // Vẽ mũi tên chỉ hướng gió
        Gizmos.color = Color.blue;
        Vector3 dir = direction.normalized;
        Vector3 center = transform.position;
        Gizmos.DrawLine(center, center + dir * 1.5f);
        Gizmos.DrawSphere(center + dir * 1.5f, 0.2f); // Đầu mũi tên
    }
}