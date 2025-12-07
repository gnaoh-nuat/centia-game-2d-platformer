using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    // Tạo lựa chọn loại trục để dễ chỉnh trong Inspector
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Cấu hình di chuyển")]
    public float speed = 2.0f;
    
    [Tooltip("Chọn trục di chuyển: Horizontal (Ngang) hoặc Vertical (Dọc)")]
    public MoveDirection direction = MoveDirection.Horizontal;

    [Tooltip("Khoảng cách di chuyển tối đa")]
    public float distance = 3.0f;

    [Tooltip("Nếu tích vào đây: Di chuyển Ngược lại (Phải sang Trái / Trên xuống Dưới). Nếu không tích: Trái sang Phải / Dưới lên Trên")]
    public bool reverse = false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        // Ghi nhớ điểm xuất phát
        startPosition = transform.position;
        
        // Tính toán điểm kết thúc dựa trên cấu hình
        CalculateEndPosition();
    }

    void Update()
    {
        // Tính toán giá trị t chạy từ 0 đến 1 rồi lại về 0 (PingPong)
        // Time.time * speed sẽ quyết định tốc độ
        // 1.0f là giá trị tối đa của t
        float t = Mathf.PingPong(Time.time * speed, 1.0f);

        // Lerp giúp di chuyển mượt mà từ startPosition đến endPosition theo giá trị t
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    }

    // Hàm tính điểm đích đến
    void CalculateEndPosition()
    {
        float dirValue = reverse ? -1f : 1f; // Nếu reverse = true thì nhân với -1 (đi ngược)

        if (direction == MoveDirection.Horizontal)
        {
            // Di chuyển theo trục X
            endPosition = startPosition + new Vector3(distance * dirValue, 0f, 0f);
        }
        else
        {
            // Di chuyển theo trục Y
            endPosition = startPosition + new Vector3(0f, distance * dirValue, 0f);
        }
    }

    // --- PHẦN XỬ LÝ VA CHẠM (GIỮ NGUYÊN) ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    
    // Vẽ đường di chuyển trong Scene để dễ nhìn (Debug)
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Khi chưa chạy game, vẽ dựa trên vị trí hiện tại giả định
            Vector3 tempEnd = transform.position;
            float dir = reverse ? -1f : 1f;
            if (direction == MoveDirection.Horizontal) tempEnd += Vector3.right * distance * dir;
            else tempEnd += Vector3.up * distance * dir;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, tempEnd);
        }
        else
        {
            // Khi đang chạy game
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}