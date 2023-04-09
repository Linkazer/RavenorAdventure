using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantiatedAnimationHandler : MonoBehaviour
{
    public enum ActionOnEnd
    {
        Destroy,
        Disable,
        DesactivateObject,
        None,
    }
    
    /// The lifespan of the animation.
    [SerializeField] private float playTime;
    [SerializeField] private ActionOnEnd endAction = ActionOnEnd.Destroy;
    [SerializeField] private RVN_AudioPlayer audioPlayer;

    public float PlayTime => playTime;

    private Action endCallback;

    [SerializeField] private UnityEvent<float> onSetTime;

    public void Play()
    {
        Play(null);
    }

    public void Play(Action callback, float duration)
    {
        //playTime = duration;

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

        if(endCallback != null && audioPlayer != null && audioPlayer.enabled)
        {
            audioPlayer.PlaySound();
        }
    }

    /// <summary>
    /// End the animation.
    /// </summary>
    public void End()
    {
        endCallback?.Invoke();

        switch(endAction)
        {
            case ActionOnEnd.Destroy:
                Destroy(gameObject);
                break;
            case ActionOnEnd.Disable:
                enabled = false;
                break;
            case ActionOnEnd.DesactivateObject:
                gameObject.SetActive(false);
                break;
            case ActionOnEnd.None:
                break;
        }
        
    }
}
