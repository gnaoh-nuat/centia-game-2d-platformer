using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Cấu hình di chuyển")]
    public float speed = 2.0f;
    public MoveDirection direction = MoveDirection.Horizontal;
    public float distance = 3.0f;
    public bool reverse = false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    // --- CÁC BIẾN MỚI ĐỂ XỬ LÝ PLAYER ---
    private PlayerController playerOnPlatform; // Tham chiếu tới script PlayerController
    private Vector3 lastPlatformPosition;      // Lưu vị trí frame trước

    void Start()
    {
        startPosition = transform.position;
        CalculateEndPosition();

        // Khởi tạo vị trí ban đầu
        lastPlatformPosition = transform.position;
    }

    // Đổi sang FixedUpdate để đồng bộ với vật lý của Player (Rigidbody)
    void FixedUpdate()
    {
        // 1. Tính toán vị trí mới của Platform
        // Lưu ý: Dùng Time.time trong FixedUpdate vẫn ổn cho chuyển động đơn giản, 
        // nhưng nếu muốn chính xác tuyệt đối về vật lý thì nên cẩn thận.
        float t = Mathf.PingPong(Time.time * speed, 1.0f);
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);

        // 2. Tính khoảng cách Platform đã di chuyển trong frame này
        Vector3 movementDelta = newPosition - lastPlatformPosition;

        // 3. Cập nhật vị trí Platform
        transform.position = newPosition;

        // 4. Nếu có Player đang đứng trên, di chuyển Player theo
        if (playerOnPlatform != null)
        {
            // Cộng khoảng cách di chuyển vào vị trí Player
            // Cách này không làm thay đổi Scale, không thay đổi cha/con
            playerOnPlatform.transform.position += movementDelta;
        }

        // 5. Cập nhật lại vị trí cũ để dùng cho frame sau
        lastPlatformPosition = newPosition;
    }

    void CalculateEndPosition()
    {
        float dirValue = reverse ? -1f : 1f;
        if (direction == MoveDirection.Horizontal)
            endPosition = startPosition + new Vector3(distance * dirValue, 0f, 0f);
        else
            endPosition = startPosition + new Vector3(0f, distance * dirValue, 0f);
    }

    // --- XỬ LÝ VA CHẠM (KHÔNG DÙNG SET PARENT) ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy component PlayerController thay vì SetParent
            playerOnPlatform = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kiểm tra kỹ xem vật rời đi có đúng là Player đang lưu không
            PlayerController exitingPlayer = collision.gameObject.GetComponent<PlayerController>();
            if (exitingPlayer != null && exitingPlayer == playerOnPlatform)
            {
                playerOnPlatform = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Vector3 tempEnd = transform.position;
            float dir = reverse ? -1f : 1f;
            if (direction == MoveDirection.Horizontal) tempEnd += Vector3.right * distance * dir;
            else tempEnd += Vector3.up * distance * dir;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, tempEnd);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}