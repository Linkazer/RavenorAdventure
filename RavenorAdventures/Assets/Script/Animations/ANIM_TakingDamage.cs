using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANIM_TakingDamage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRnd;
    [SerializeField] private float animationTime;

    //TODO : Refaire un script plus global.
    public void Play()
    {
        sprRnd.color = Color.red;
        TimerManager.CreateGameTimer(animationTime, () => sprRnd.color = Color.white);
    }
}
