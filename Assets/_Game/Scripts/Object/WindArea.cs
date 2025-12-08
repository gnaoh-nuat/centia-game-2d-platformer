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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 force = direction.normalized * strength * Time.deltaTime * 50f;
                rb.AddForce(force);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);

        Gizmos.color = Color.blue;
        Vector3 dir = direction.normalized;
        Vector3 center = transform.position;
        Gizmos.DrawLine(center, center + dir * 1.5f);
        Gizmos.DrawSphere(center + dir * 1.5f, 0.2f);
    }
}