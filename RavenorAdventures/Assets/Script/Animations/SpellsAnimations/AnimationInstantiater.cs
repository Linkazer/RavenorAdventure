using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInstantiater : RVN_Singleton<AnimationInstantiater>
{
    public static void PlayAnimationAtPosition(InstantiatedAnimationHandler toPlay, Vector2 position, Action callback = null)
    {
        InstantiatedAnimationHandler runtimePlayedAnimation = Instantiate(toPlay, position, Quaternion.identity);

        runtimePlayedAnimation.Play(callback);
    }
}
