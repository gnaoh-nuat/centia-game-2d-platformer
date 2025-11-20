using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        player.JumpLeft--;
        float force = player.JumpForce;

        base.Enter();
        Debug.Log("Entered Jump State");

        if (player.JumpLeft < player.MaxJumps - 1)
        {
            force *= player.DoubleJumpMultiplier;
        }

        player.SetVelocityY(force);

        inputReader.JumpCanceledEvent += OnJumpCanceled;
        inputReader.JumpEvent += OnAirJump;
        inputReader.DashEvent += OnDashPressed;
    }

    public override void Exit()
    {
        base.Exit();
        inputReader.JumpCanceledEvent -= OnJumpCanceled;
        inputReader.JumpEvent -= OnAirJump;
        inputReader.DashEvent -= OnDashPressed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.Rigidbody2D.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(player.MoveInput.x * player.MoveSpeed);
    }

    // Handle jump cancellation for variable jump height
    private void OnJumpCanceled()
    {
        if (player.Rigidbody2D.linearVelocity.y > 0)
        {
            player.SetVelocityY(player.Rigidbody2D.linearVelocity.y * 0.5f);
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
