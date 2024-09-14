using System;
using UnityEngine;

[Serializable]
public class AudioModel
{

    public string name;
    public AudioClip clip;
    public AudioSource audioSource;

    public bool PlayOnAwake = false;
    public float Volume = 1f;
    public float Pitch = 1f;



    public void InitializeAudioSource()
    {
        audioSource.clip = clip;
        audioSource.volume = Volume;
        audioSource.pitch = Pitch;
        audioSource.playOnAwake = PlayOnAwake;
    }
    internal void Play()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    internal void Stop()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}