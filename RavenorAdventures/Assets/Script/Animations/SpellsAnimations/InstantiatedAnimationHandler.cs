using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedAnimationHandler : MonoBehaviour
{
    [SerializeField] private float playTime;
    public float PlayTime => playTime;

    private Action endCallback;

    public void Play(Action callback)
    {
        endCallback = callback;
        if (playTime > 0)
        {
            TimerManager.CreateGameTimer(PlayTime, End);
        }
    }

    public void End()
    {
        endCallback?.Invoke();
        Destroy(gameObject);
    }
}
