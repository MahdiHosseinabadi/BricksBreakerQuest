using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Shoot,
    BrickHit,
    BrickDestroy,
    WallHit,
    BallCollect,
    NextRound,
    GameOver
}

[System.Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.5f, 2f)]
    public float pitch;

    [Tooltip("Minimum interval between two audio playing")]
    public float coolDown = 0f;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip musicBackground;
    public float volumeMusicBackground;

    public Sound[] sounds;

    Dictionary<SoundType, Sound> SoundDictionary;
    Dictionary<SoundType, float> LastPlayTime;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SoundDictionary = new Dictionary<SoundType, Sound>();
        LastPlayTime = new Dictionary<SoundType, float>();

        foreach (Sound sound in sounds)
        {
            SoundDictionary.Add(sound.type, sound);
            LastPlayTime.Add(sound.type, -999f);
        }
    }

    void Start()
    {
        PlayMusic(musicBackground);
        SetMusicVolume(volumeMusicBackground);
    }

    public void Play(SoundType type)
    {
        if (!UIManager.instance.SFXEnabled) return;

        if (!SoundDictionary.TryGetValue(type, out Sound sound)) return;

        if (Time.time - LastPlayTime[type] < sound.coolDown) return;

        LastPlayTime[type] = Time.time;

        sfxSource.pitch = sound.pitch;
        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();
    public void PauseMusic() => musicSource.Pause();
    public void UnPauseMusic() => musicSource.UnPause();

    public void SetMusicVolume(float volume) => musicSource.volume = Mathf.Clamp01(volume);
    public void SetSFXVolume(float volume) => sfxSource.volume = Mathf.Clamp01(volume);
}