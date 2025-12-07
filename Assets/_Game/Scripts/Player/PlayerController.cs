using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Animator _animator;
    public Rigidbody2D Rigidbody2D { get; private set; }
    public StaminaSystem Stamina { get; private set; }
    public HealthSystem Health { get; private set; }

    [Header("Config Data")]
    public CharacterStatsSO Stats;

    public int AnimID_Idle { get; private set; }
    public int AnimID_Run { get; private set; }
    public int AnimID_Jump { get; private set; }
    public int AnimID_Fall { get; private set; }
    public int AnimID_Die { get; private set; }
    public int AnimID_Hurt { get; private set; }
    public int AnimID_Dash { get; private set; }


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
    [SerializeField] private BoxCollider2D _playerCollider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _groundCheckWidthScale = 0.9f;

    // State Machine
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public int FacingDirection { get; private set; } = 1; // 1 = right, -1 = left

    private float _dashReadyTime; // Time when dash will be ready again

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Stamina = GetComponent<StaminaSystem>();
        Health = GetComponent<HealthSystem>();

        // Initialize Animation IDs
        AnimID_Idle = Animator.StringToHash("Idle");
        AnimID_Run = Animator.StringToHash("Run");
        AnimID_Jump = Animator.StringToHash("Jump");
        AnimID_Fall = Animator.StringToHash("Fall");
        AnimID_Die = Animator.StringToHash("Die");
        AnimID_Hurt = Animator.StringToHash("Hurt");
        AnimID_Dash = Animator.StringToHash("Dash");

        // Initialize State Machine and States
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, _inputReader);
        MoveState = new PlayerMoveState(this, StateMachine, _inputReader);
        JumpState = new PlayerJumpState(this, StateMachine, _inputReader);
        DashState = new PlayerDashState(this, StateMachine, _inputReader);
        FallState = new PlayerFallState(this, StateMachine, _inputReader);
        HurtState = new PlayerHurtState(this, StateMachine, _inputReader);
        DeathState = new PlayerDeathState(this, StateMachine, _inputReader);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += HandledMoveInput;

        Health.OnDamaged += HandleDamaged;
        Health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= HandledMoveInput;

        Health.OnDamaged -= HandleDamaged;
        Health.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    // Handle movement input
    private void HandledMoveInput(Vector2 input)
    {
        MoveInput = input;

        if (input.x != 0)
        {
            FacingDirection = input.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(FacingDirection, 1, 1);
        }
    }

    private void HandleDamaged(Vector2 knockback)
    {
        Rigidbody2D.linearVelocity = knockback;

        StateMachine.ChangeState(HurtState);
    }

    private void HandleDeath()
    {
        StateMachine.ChangeState(DeathState);
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

    public void PlayAnimation(int animID)
    {
        _animator.CrossFade(animID, 0f);
    }

    public bool IsGrounded()
    {
        Bounds bounds = _playerCollider.bounds;

        Vector2 boxSize = new Vector2(bounds.size.x * _groundCheckWidthScale, 0.1f);

        // Perform BoxCast downwards to check for ground
        RaycastHit2D hit = Physics2D.BoxCast(
            bounds.center, 
            boxSize, 
            0f,
            Vector2.down,
            (bounds.size.y / 2) + _groundCheckDistance, 
            _groundLayer
        );

        return hit.collider != null;
    }

    public bool CanDash()
    {
        if (Time.time < _dashReadyTime) return false;
        //float dashCost = Stats.DashStaminaCost;
        //if (Stamina.CurrentStamina < dashCost) return false;
        return true;
    }

    // Visualize ground check in editor
    private void OnDrawGizmos()
    {
        if (_playerCollider == null) return;

        Bounds bounds = _playerCollider.bounds;

        Vector2 boxSize = new Vector2(bounds.size.x * _groundCheckWidthScale, 0.1f);

        float yOffset = (bounds.size.y / 2) + _groundCheckDistance;
        Vector2 centerPoint = new Vector2(bounds.center.x, bounds.center.y - yOffset);

        Gizmos.color = IsGrounded() ? Color.green : Color.red; // Green if grounded, red if not
        Gizmos.DrawWireCube(centerPoint, boxSize);
    }
}
