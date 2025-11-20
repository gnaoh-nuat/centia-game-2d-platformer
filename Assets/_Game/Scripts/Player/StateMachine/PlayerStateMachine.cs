using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    // Initialize the state machine with the starting state
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // Change to a new state
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
