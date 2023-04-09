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
        source.clip = audioData.Clip;
        source.Play();
    }
}
