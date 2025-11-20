using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    public Rigidbody2D Rigidbody2D { get; private set; }

    [Header("Movement Settings")]
    public float MoveSpeed = 5f;

    [Header("Jump Settings")]
    public float JumpForce = 10f;
    public int MaxJumps = 2;
    public int JumpLeft { get; set; }
    [Range(0f, 1f)]
    public float DoubleJumpMultiplier = 0.8f;

    [Header("Dash Settings")]
    public float DashSpeed = 10f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f); // Width and height of the ground check box
    [SerializeField] private LayerMask _groundLayer;

    // State Machine
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public int FacingDirection { get; private set; } = 1; // 1 = right, -1 = left

    private float _dashReadyTime; // Time when dash will be ready again

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        // Initialize State Machine and States
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, _inputReader);
        MoveState = new PlayerMoveState(this, StateMachine, _inputReader);
        JumpState = new PlayerJumpState(this, StateMachine, _inputReader);
        DashState = new PlayerDashState(this, StateMachine, _inputReader);
        FallState = new PlayerFallState(this, StateMachine, _inputReader);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += HandledMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= HandledMoveInput;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    // Handle move input from InputReader
    private void HandledMoveInput(Vector2 input)
    {
        MoveInput = input;

        if (input.x != 0)
        {
            FacingDirection = input.x > 0 ? 1 : -1;
        }
    }

    public void SetVelocityX(float velocity)
    {
        Rigidbody2D.linearVelocity = new Vector2(velocity, Rigidbody2D.linearVelocity.y);
    }

    public void SetVelocityY(float velocity)
    {
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, velocity);
    }

    public void ResetJumpCounter()
    {
        JumpLeft = MaxJumps;
    }

    public void ResetDashCooldown()
    {
        _dashReadyTime = Time.time + DashCooldown;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, _groundLayer) != null;
    }

    public bool CanDash()
    {
        return Time.time >= _dashReadyTime;
    }

    // Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        }
    }
}
