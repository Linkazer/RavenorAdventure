using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CANIM_Animator : CharacterAnimation
{
    [SerializeField] protected Animator animator;

    public override void Play(object animationdata)
    {
        animator.Play($"Base Layer.{currentAnimation.AnimationName}");
        TimerManager.CreateGameTimer(Time.deltaTime, SetAnimationLength);
    }

    public override void Stop()
    {
        animator.Play("Base Layer.Character_Idle");
    }

    public override void End()
    {
        Stop();

        if(currentAnimation.linkedAnimation != "")
        {
            currentAnimation.endCallback?.Invoke();
        }
    }

    private void SetAnimationLength()
    {
        if(!currentAnimation.doesLoop)
        {
            TimerManager.CreateGameTimer(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - Time.deltaTime, End);
        }
    }
}
