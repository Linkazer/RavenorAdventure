using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    [SerializeField] protected CPN_ANIM_Character animationHandler;

    protected CharacterAnimationData currentAnimation;

    protected List<AudioClip> audioClips = new List<AudioClip>();

    public virtual AudioClip GetClip()
    {
        if (audioClips.Count > 0)
        {
            return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
        }
        return null;
    }

    public void Play(object animationData, CharacterAnimationData characterAnimationData)
    {
        currentAnimation = characterAnimationData;

        if(animationData is ISoundHolder)
        {
            audioClips = (animationData as ISoundHolder).GetClips();
        }
        else
        {
            audioClips = new List<AudioClip>();
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
