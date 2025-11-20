using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected InputReader inputReader;

    // Called when the state is entered
    public PlayerState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.inputReader = inputReader;
    }

    // State lifecycle methods
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { } // Called every frame
    public virtual void PhysicsUpdate() { } // Called every physics update
}
