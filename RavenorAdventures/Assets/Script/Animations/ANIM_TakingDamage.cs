using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANIM_TakingDamage : CharacterAnimation
{
    [SerializeField] private SpriteRenderer sprRnd;
    [SerializeField] private float animationTime;

    public void Play()
    {
        sprRnd.color = Color.red;
        TimerManager.CreateGameTimer(animationTime, () => sprRnd.color = Color.white);
    }

    public override void Play(object animationdata)
    {
        Play();
    }

    public override void Stop()
    {
        
    }

    public override void End()
    {

    }
}
