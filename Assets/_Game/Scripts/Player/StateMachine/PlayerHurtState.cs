using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private float _hurtDuration = 0.5f; // Thời gian bị choáng
    private float _timer;

    public PlayerHurtState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player Hurt State Entered");
        player.PlayAnimation(player.AnimID_Hurt);
        _timer = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _timer += Time.deltaTime;

        if (_timer >= _hurtDuration)
        {
            if (player.IsGrounded())
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}