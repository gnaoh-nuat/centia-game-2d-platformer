using UnityEngine;
using System;
public class Checkpoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;
    private const string IS_ACTIVE_PARAM = "isActive";

    public static event Action<Checkpoint> OnCheckpointActivated;

    private void OnEnable()
    {
        OnCheckpointActivated += HandleCheckpointState;
    }

    private void OnDisable()
    {
        OnCheckpointActivated -= HandleCheckpointState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        OnCheckpointActivated?.Invoke(this);

        GameManager.Instance.UpdateCheckpoint(transform.position);
    }

    private void HandleCheckpointState(Checkpoint currentActiveCheckpoint)
    {
        if (currentActiveCheckpoint == this)
        {
            _animator.SetBool(IS_ACTIVE_PARAM, true);
        }
        else
        {
            _animator.SetBool(IS_ACTIVE_PARAM, false);
        }
    }
}