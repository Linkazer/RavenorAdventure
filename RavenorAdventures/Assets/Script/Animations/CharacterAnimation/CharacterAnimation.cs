using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    protected CharacterAnimationData currentAnimation;

    public void Play(object animationData, CharacterAnimationData characterAnimationData)
    {
        currentAnimation = characterAnimationData;
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
    public override void Play(object animationdata)
    {
        if(animationdata.GetType() == typeof(T))
        {
            Play((T)animationdata);
        }
    }

    public abstract void Play(T animationdata);
}
