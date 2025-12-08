using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Cấu hình di chuyển")]
    public float speed = 1.0f;                  // Vận tốc thật (mỗi giây)
    public MoveDirection direction = MoveDirection.Horizontal;
    public float distance = 3.0f;
    public bool reverse = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPos;

    // --- CÁC BIẾN DÙNG CHO PLAYER ---
    private PlayerController playerOnPlatform;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        startPosition = transform.position;
        CalculateEndPosition();

        // Điểm đích đầu tiên
        targetPos = endPosition;

        lastPlatformPosition = transform.position;
    }

    void FixedUpdate()
    {
        // --- 1. Di chuyển Platform theo vận tốc thật ---
        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // --- 2. Tính vector di chuyển của Platform ---
        Vector3 movementDelta = newPosition - lastPlatformPosition;

        // --- 3. Cập nhật vị trí Platform ---
        transform.position = newPosition;

        // --- 4. Nếu Player đang đứng trên thì di chuyển theo ---
        if (playerOnPlatform != null)
        {
            playerOnPlatform.transform.position += movementDelta;
        }

        // --- 5. Đổi hướng khi chạm điểm đích ---
        if (Vector3.Distance(transform.position, targetPos) <= 0.02f)
        {
            targetPos = (targetPos == endPosition) ? startPosition : endPosition;
        }

        // --- 6. Lưu lại vị trí trước ---
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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

            if (direction == MoveDirection.Horizontal)
                tempEnd += Vector3.right * distance * dir;
            else
                tempEnd += Vector3.up * distance * dir;

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
