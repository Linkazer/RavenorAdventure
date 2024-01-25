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

        private bool isRealTime =  false;

        public Coroutine coroutine;

        public Action Callback => callback;

        public float Duration => duration;

        public float DurationLeft => duration - (Time.time - startTime);

        public bool IsRealTime => isRealTime;

        public void SetAsGame(float duration, Action nCallback)
        {
            startTime = Time.time;
            this.duration = duration;
            callback = nCallback;
            isRealTime = false;
        }

        public void SetAsReal(float duration, Action nCallback)
        {
            startTime = Time.realtimeSinceStartup;
            this.duration = duration;
            callback = nCallback;
            isRealTime = true;
        }

        public void Pause()
        {
            StopTimerRoutine(coroutine);
            coroutine = null;
            duration = DurationLeft;
        }

        public void Restart()
        {
            if (coroutine == null)
            {
                StartTimerRoutine(this);
            }
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
        StartTimerRoutine(toReturn);
        return toReturn;
    }

    public static Timer CreateRealTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.SetAsReal(time, callback);
        StartTimerRoutine(toReturn);
        return toReturn;
    }

    private static void StopTimerRoutine(Coroutine toStop)
    {
        if (toStop != null && instance != null)
        {
            instance.StopCoroutine(toStop);
        }
    }

    private static void StartTimerRoutine(Timer timerToStart)
    {
        if (timerToStart.IsRealTime)
        {
            timerToStart.coroutine = instance.StartCoroutine(instance.RealTimerRoutine(timerToStart));
        }
        else
        {
            timerToStart.coroutine = instance.StartCoroutine(instance.GameTimerRoutine(timerToStart));
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
