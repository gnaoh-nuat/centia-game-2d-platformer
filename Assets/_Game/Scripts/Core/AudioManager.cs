using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- PHÁT NHẠC NỀN (Loop) ---
    public void PlayMusic(AudioClip clip)
    {
        // Nếu nhạc này đang phát rồi thì thôi (tránh bị reset bài hát)
        if (_musicSource.clip == clip) return;

        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    // --- PHÁT HIỆU ỨNG (One Shot) ---
    // PlayOneShot giúp phát chồng nhiều âm thanh lên nhau (ví dụ: tiếng súng bắn liên thanh)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    // Tùy chọn: Phát SFX với độ cao (Pitch) ngẫu nhiên để đỡ nhàm chán (dùng cho tiếng bước chân, nhảy)
    public void PlaySFXRandomPitch(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.pitch = Random.Range(0.9f, 1.1f);
            _sfxSource.PlayOneShot(clip);
            _sfxSource.pitch = 1f; // Reset lại pitch chuẩn
        }
    }
}