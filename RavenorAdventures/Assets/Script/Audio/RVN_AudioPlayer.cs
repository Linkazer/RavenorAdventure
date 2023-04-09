using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RVN_AudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clips;

    [SerializeField] private AudioSource source;

    public void PlaySound(AudioClip toPlay)
    {
        if (toPlay != null)
        {
            source.clip = toPlay;
            source.Play();
        }
    }

    public void PlaySound()
    {
        PlaySound(clips[Random.Range(0, clips.Count)]);
    }
}
