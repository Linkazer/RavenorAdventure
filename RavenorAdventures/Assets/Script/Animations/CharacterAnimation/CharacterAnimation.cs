using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    [SerializeField] protected CPN_ANIM_Character animationHandler;

    protected CharacterAnimationData currentAnimation;

    protected AudioData audioData;

    public virtual AudioData GetClip()
    {
        if (audioData != null)
        {
            return audioData;
        }
        return null;
    }

    public void Play(object animationData, CharacterAnimationData characterAnimationData)
    {
        currentAnimation = characterAnimationData;

        if(animationData is ISoundHolder)
        {
            audioData = (animationData as ISoundHolder).GetAudioData();
        }
        else
        {
            audioData = null;
        }

        Play(animationData);
    }

    public abstract void Play(object animationdata);

    public abstract void Stop();

    public abstract void End();

    public virtual void SetCharacter(CharacterScriptable_Battle character)
    {

    }
}

public abstract class CharacterAnimation<T> : CharacterAnimation
{
    protected T data;

    public override void Play(object animationdata)
    {
        if(animationdata.GetType() == typeof(T))
        {
            data = (T)animationdata;
            Play(data);
        }
    }

    public abstract void Play(T animationdata);
}
