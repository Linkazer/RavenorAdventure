using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioClip[] clips;

    public AudioClip[] Clips => clips;
    public AudioClip Clip => clips[Random.Range(0, clips.Length)];
}
