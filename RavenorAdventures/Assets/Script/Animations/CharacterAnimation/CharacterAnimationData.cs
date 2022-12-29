using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAnimationData
{
    [SerializeField] private string animationName;
    [SerializeField] private CharacterAnimation animationUsed;
    public bool doesLoop;
    public string linkedAnimation = "";

    public Action endCallback;

    public string AnimationName => animationName;

    public void Play(object animationdata, Action callback)
    {
        endCallback = callback;

        animationUsed.Play(animationdata, this);
    }

    public void Stop()
    {
        animationUsed.Stop();
    }
}
