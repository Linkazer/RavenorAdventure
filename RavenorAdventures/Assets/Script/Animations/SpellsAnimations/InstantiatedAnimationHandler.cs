using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantiatedAnimationHandler : MonoBehaviour
{
    /// The lifespan of the animation.
    [SerializeField] private float playTime;
    public float PlayTime => playTime;

    private Action endCallback;

    [SerializeField] private UnityEvent<float> onSetTime;

    public void Play(Action callback, float duration)
    {
        playTime = duration;

        onSetTime?.Invoke(1/PlayTime);

        Play(callback);
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public void Play(Action callback)
    {
        endCallback = callback;
        if (playTime > 0)
        {
            TimerManager.CreateGameTimer(PlayTime, End);
        }
    }

    /// <summary>
    /// End the animation.
    /// </summary>
    public void End()
    {
        endCallback?.Invoke();
        Destroy(gameObject);
    }
}
