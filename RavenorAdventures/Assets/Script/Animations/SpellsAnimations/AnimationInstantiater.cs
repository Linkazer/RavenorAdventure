using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for every animation or VFX that need to be instantiate.
/// </summary>
public class AnimationInstantiater : RVN_Singleton<AnimationInstantiater>
{
    /// <summary>
    /// Instatiate an Animation object at the wanted position for a certain amount of time.
    /// </summary>
    /// <param name="toPlay">The animation object to instantiate.</param>
    /// <param name="duration">The lifespan of the Animation object.</param>
    /// <param name="position">The position where the animation is played.</param>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public static void PlayAnimationAtPosition(InstantiatedAnimationHandler toPlay, float duration, Vector2 position, Action callback = null)
    {
        InstantiatedAnimationHandler runtimePlayedAnimation = Instantiate(toPlay, position, Quaternion.identity);

        runtimePlayedAnimation.Play(callback, duration);
    }

    /// <summary>
    /// Instatiate an Animation object at the wanted position.
    /// </summary>
    /// <param name="toPlay">The animation object to instantiate.</param>
    /// <param name="position">The position where the animation is played.</param>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public static void PlayAnimationAtPosition(InstantiatedAnimationHandler toPlay, Vector2 position, Action callback = null)
    {
        InstantiatedAnimationHandler runtimePlayedAnimation = Instantiate(toPlay, position, Quaternion.identity);

        runtimePlayedAnimation.Play(callback);
    }
}
