using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(_clickSound);
        }
    }
}
