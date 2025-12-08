using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader", order = 1)]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // Events
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction JumpEvent;
    public event UnityAction JumpCanceledEvent; // Added event for jump canceled
    public event UnityAction DashEvent;
    public event UnityAction PauseEvent;

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            // Set this class as the callback receiver for gameplay actions
            _gameInput.Gameplay.SetCallbacks(this); 
        }
        
        _gameInput.Gameplay.Enable(); // Enable the gameplay action map
    }

    private void OnDisable()
    {
        _gameInput.Gameplay.Disable(); // Disable the gameplay action map
    }

    // Gameplay action callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
        }
        else if (context.canceled)
        {
            JumpCanceledEvent?.Invoke(); // Invoke the jump canceled event
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }
}
