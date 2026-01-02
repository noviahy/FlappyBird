using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip bgmClip;
    public AudioClip coin;
    public AudioClip jump;
    public AudioClip hit;
    public AudioClip click;
    public AudioClip countDown;
    public AudioClip start;
    public AudioClip buy;

    private void Awake()
    {
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

    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
