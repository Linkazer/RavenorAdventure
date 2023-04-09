using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAnimationData
{
    [SerializeField] private string animationName;
    [SerializeField] private CharacterAnimation animationUsed;
    [SerializeField] private List<AudioClip> clips;
    public bool doesLoop;
    public string linkedAnimation = "";

    public Action endCallback;

    public string AnimationName => animationName;

    public void Play(object animationdata, Action callback)
    {
        endCallback = callback;

        animationUsed.Play(animationdata, this);
    }

    public AudioClip GetClip()
    {
        AudioClip toReturn = animationUsed.GetClip();

        if(toReturn == null && clips.Count > 0)
        {
            toReturn = clips[UnityEngine.Random.Range(0, clips.Count)];
        }

        return toReturn;
    }

    public void Stop()
    {
        animationUsed.Stop();
    }
}
