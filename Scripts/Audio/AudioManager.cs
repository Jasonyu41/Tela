using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    const float PITCH_MIN = 0.9f;
    const float PITCH_MAX = 1.1f;
    
    float originalPitch;

    protected override void Awake()
    {
        base.Awake();

        originalPitch = sFXPlayer.pitch;
    }

    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    public void PlaySFX(AudioData[] audioData)
    {
        PlaySFX(audioData[Random.Range(0, audioData.Length)]);
    }

    public void PlayRandomPitchSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(PITCH_MIN, PITCH_MAX);
        PlaySFX(audioData);
        sFXPlayer.pitch = originalPitch;
    }

    public void PlayRandomPitchSFX(AudioData[] audioData)
    {
        PlayRandomPitchSFX(audioData[Random.Range(0, audioData.Length)]);
    }
}

[System.Serializable]
public struct AudioData
{
    public AudioClip audioClip;
    [Range(0f, 100f)] public float volume;
}