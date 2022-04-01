using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : RVN_Singleton<TimerManager>
{
    public class Timer
    {
        private float duration;

        private Action callback = null;

        private float startTime;

        public Coroutine coroutine;

        public Action Callback => callback;

        public float Duration => duration;

        public float DurationLeft => duration - (Time.time - startTime);

        public void SetAsGame(float duration, Action nCallback)
        {
            startTime = Time.time;
            this.duration = duration;
            callback = nCallback;
        }

        public void SetAsReal(float duration, Action nCallback)
        {
            startTime = Time.realtimeSinceStartup;
            this.duration = duration;
            callback = nCallback;
        }

        public void Stop()
        {
            StopTimerRoutine(coroutine);
        }

        public void Execute()
        {
            callback?.Invoke();
            Stop();
        }
    }


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static Timer CreateGameTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.SetAsGame(time, callback);
        toReturn.coroutine = instance.StartCoroutine(instance.GameTimerRoutine(toReturn));
        return toReturn;
    }

    public static Timer CreateRealTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.SetAsReal(time, callback);
        toReturn.coroutine = instance.StartCoroutine(instance.RealTimerRoutine(toReturn));
        return toReturn;
    }

    private static void StopTimerRoutine(Coroutine toStop)
    {
        if (toStop != null && instance != null)
        {
            instance.StopCoroutine(toStop);
        }
    }

    IEnumerator GameTimerRoutine(Timer timer)
    {
        yield return new WaitForSeconds(timer.Duration);
        timer.Execute();
    }

    IEnumerator RealTimerRoutine(Timer timer)
    {
        yield return new WaitForSecondsRealtime(timer.Duration);
        timer.Execute();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
