using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePatrolEnemy : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public Transform PointA;
    public Transform PointB;
    public float Speed = 3f;
    public float WaitTime = 1f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;
    private Transform _currentTarget;
    private float _waitCounter;
    private bool _isWaiting;

    // Animation IDs
    private int _animID_Idle;
    private int _animID_Walk;
    private int _currentAnimID;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();

        _animID_Idle = Animator.StringToHash("Idle");
        _animID_Walk = Animator.StringToHash("Walk");
    }

    private void Start()
    {
        _currentTarget = PointA;
        _isWaiting = false;

        // Cập nhật hướng mặt ngay từ đầu
        Flip();
        PlayAnim(_animID_Walk);
    }

    private void FixedUpdate()
    {
        // 1. Xử lý logic Đứng nghỉ
        if (_isWaiting)
        {
            PlayAnim(_animID_Idle);

            // Khóa vận tốc lại để không bị trượt
            _rb.linearVelocity = Vector2.zero;

            _waitCounter -= Time.fixedDeltaTime;
            if (_waitCounter <= 0)
            {
                _isWaiting = false;
                SwitchTarget(); // Đổi mục tiêu
                Flip();         // Quay đầu
            }
            return;
        }

        // 2. Xử lý logic Di chuyển
        Move();
    }

    private void Move()
    {
        PlayAnim(_animID_Walk);

        // Tính khoảng cách chỉ trên trục X (để tránh lỗi do lệch độ cao Y)
        float distanceX = Mathf.Abs(_currentTarget.position.x - transform.position.x);

        // Nếu khoảng cách đủ nhỏ -> Đã đến nơi
        if (distanceX < 0.2f)
        {
            _isWaiting = true;
            _waitCounter = WaitTime;
            _rb.linearVelocity = Vector2.zero; // Dừng ngay lập tức
            return;
        }

        // Di chuyển
        // Lấy vị trí mục tiêu X, nhưng giữ nguyên Y của bản thân (để không bay lên/xuống theo điểm mốc)
        Vector2 targetPos = new Vector2(_currentTarget.position.x, _rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(_rb.position, targetPos, Speed * Time.fixedDeltaTime);
        _rb.MovePosition(newPos);
    }

    private void SwitchTarget()
    {
        if (_currentTarget == PointA)
            _currentTarget = PointB;
        else
            _currentTarget = PointA;
    }

    private void Flip()
    {
        // Xác định hướng: Nếu mục tiêu nằm bên Phải (> position.x) thì direction = 1, ngược lại -1
        // LƯU Ý: Nếu sprite của bạn bị ngược (đi phải mà mặt quay trái), hãy thêm dấu trừ (-) vào trước biểu thức này
        float direction = _currentTarget.position.x > transform.position.x ? 1 : -1;

        // Sửa lỗi: Nếu Sprite gốc của bạn quay mặt sang TRÁI, hãy đổi dòng trên thành:
        // float direction = _currentTarget.position.x > transform.position.x ? -1 : 1;

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void PlayAnim(int animID)
    {
        if (_currentAnimID == animID) return;
        _animator.CrossFade(animID, 0f);
        _currentAnimID = animID;
    }

    private void OnDrawGizmos()
    {
        if (PointA != null && PointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(PointA.position, 0.3f);
            Gizmos.DrawWireSphere(PointB.position, 0.3f);
            Gizmos.DrawLine(PointA.position, PointB.position);
        }
    }
}