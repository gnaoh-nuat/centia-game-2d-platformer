using UnityEngine;
using Unity.Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _levelMusic; 

    private void Start()
    {
        if (_levelMusic != null)
        {
            AudioManager.Instance.PlayMusic(_levelMusic);
        }
    }
}