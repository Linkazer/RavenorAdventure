using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RVN_AudioPlayer : MonoBehaviour
{
    [SerializeField] protected AudioData audioData;
    [SerializeField] protected AudioSource source;

    public void PlaySound(AudioData toPlay)
    {
        if (toPlay != null)
        {
            source.clip = toPlay.Clip;
            source.Play();
        }
    }

    public void PlaySound()
    {
        if (audioData != null)
        {
            source.clip = audioData.Clip;
            source.Play();
        }
        else
        {
            Debug.Log("Miss audio data on : " + gameObject);
        }
    }

    public void StopSound()
    {
        source.Stop();
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
    }
}
