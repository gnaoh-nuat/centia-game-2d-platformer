using UnityEngine;

public class PlayerMoveState : PlayerState
{
    // Constructor
    public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Move State");

        player.ResetJumpCounter();

        inputReader.JumpEvent += OnJumpPressed;
        inputReader.DashEvent += OnDashPressed;
    }

    public override void Exit()
    {
        base.Exit();
        inputReader.JumpEvent -= OnJumpPressed;
        inputReader.DashEvent -= OnDashPressed;
    }

    public override void LogicUpdate()
    {
        if (player.MoveInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        player.SetVelocityX(player.MoveInput.x * player.MoveSpeed);
    }

    private void OnJumpPressed()
    {
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }
    private void OnDashPressed()
    {
        if (player.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }
}
