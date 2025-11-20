using UnityEngine;

public class PlayerIdleState : PlayerState
{
    // Constructor
    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Idle State");
        player.SetVelocityX(0);

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
        // Transition to Move State if there is movement input
        if (player.MoveInput.x != Vector2.zero.x)
        {
            stateMachine.ChangeState(player.MoveState);
        }
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
