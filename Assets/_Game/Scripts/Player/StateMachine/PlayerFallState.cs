using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Fall State");

        inputReader.JumpEvent += OnAirJump;
        inputReader.DashEvent += OnDashPressed;
    }

    public override void Exit()
    {
        base.Exit();

        inputReader.JumpEvent -= OnAirJump;
        inputReader.DashEvent -= OnDashPressed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.IsGrounded())
        {
            if (player.MoveInput.x == 0)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(player.MoveInput.x * player.MoveSpeed);

        if (player.Rigidbody2D.linearVelocity.y < -20f)
        {
            player.SetVelocityY(-20f); // Cap the fall speed
        }
    }
    private void OnAirJump()
    {
        if (player.JumpLeft > 0)
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
