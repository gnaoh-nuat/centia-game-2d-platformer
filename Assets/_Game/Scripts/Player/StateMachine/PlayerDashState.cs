using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float _startTime;
    private float _originalGravityScale;

    public PlayerDashState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Dash State");

        AudioManager.Instance.PlaySFX(player.DashSFX);

        player.PlayAnimation(player.AnimID_Dash);

        // Record the start time of the dash
        _startTime = Time.time;

        // Turn off gravity during dash
        _originalGravityScale = player.Rigidbody2D.gravityScale;
        player.Rigidbody2D.gravityScale = 0f;

        // Determine dash direction
        int dashDirection = player.FacingDirection;
        if (player.MoveInput.x != 0)
        {
            dashDirection = player.MoveInput.x > 0 ? 1 : -1;
        }

        player.SetVelocityX(dashDirection * player.DashSpeed);
        player.SetVelocityY(0f); // Neutralize vertical velocity
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exited Dash State");

        player.Rigidbody2D.gravityScale = _originalGravityScale; // Restore original gravity scale
        player.SetVelocityX(0f); // Stop horizontal movement after dash
        player.ResetDashCooldown();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Check if dash duration has elapsed
        if (Time.time >= _startTime + player.DashDuration)
        {
            // Transition to appropriate state after dash
            if (player.IsGrounded())
            {
                if (player.MoveInput.x == 0)
                    stateMachine.ChangeState(player.IdleState);
                else
                    stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.FallState);
            }
        }
    }
}
