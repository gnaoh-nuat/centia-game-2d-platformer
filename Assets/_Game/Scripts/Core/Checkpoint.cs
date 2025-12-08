using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _activeColor = Color.green;
    [SerializeField] private Color _inactiveColor = Color.white;

    private bool _isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated) return;

        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        _isActivated = true;
        if (_renderer != null) _renderer.color = _activeColor;

        GameManager.Instance.UpdateCheckpoint(transform.position);
    }

}