using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;


public class RoundTimer
{
    public TimerManager.Timer timer;
    public float maximumRound;
    public float roundLeft;

    private Action onTimerUpdate;
    private Action onTimerEnd;

    public RoundTimer(float rounds, Action updateCallback, Action endCallback)
    {
        maximumRound = rounds;
        roundLeft = maximumRound;

        onTimerUpdate = updateCallback;
        onTimerEnd = endCallback;
    }

    public void StartTimer()
    {
        float timerRoundDuration = RVN_RoundManager.Instance.RealTimeRoundDuration;

        if (roundLeft < 1)
        {
            timerRoundDuration *= roundLeft;
        }

        timer = TimerManager.CreateGameTimer(timerRoundDuration, TimerRoundEnd);
    }

    public void StopTimer()
    {
        if (timer != null)
        {
            roundLeft -= timer.Duration - timer.DurationLeft;

            timer.Stop();
            timer = null;
        }
    }

    public void ProgressTimer(float roundsToProgress)
    {
        roundLeft -= roundsToProgress;

        CheckTimerEnd();
    }

    private void TimerRoundEnd()
    {
        roundLeft--;

        CheckTimerEnd();
    }

    private void CheckTimerEnd()
    {
        if (roundLeft > 0)
        {
            timer = null;
            if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime)
            {
                onTimerUpdate?.Invoke();
            
                StartTimer();
            }
        }
        else
        {
            TimerEnd();
        }
    }

    private void TimerEnd()
    {
        onTimerEnd?.Invoke();

        RVN_RoundManager.Instance.RemoveTimer(this);
    }
}

public class RVN_RoundManager : RVN_Singleton<RVN_RoundManager>
{
    public enum RoundMode
    {
        Round,
        RealTime,
    }

    [Header("Real time")]
    private float realTimeRoundDuration = 6f;

    private RoundMode currentRoundMode;

    private List<RVN_ComponentHandler> activeHandlers = new List<RVN_ComponentHandler>();
    private List<RoundTimer> activeTimers = new List<RoundTimer>();

    private bool isPaused = false;

    private RoundTimer realTimeGeneralRoundTimer = null;

    public Action<RoundMode> actOnUpdateRoundMode;

    public bool IsPaused => isPaused;

    public float RealTimeRoundDuration => realTimeRoundDuration;

    public RoundMode CurrentRoundMode => currentRoundMode;

    public RoundTimer CreateTimer(float rounds, Action updateCallback, Action endCallback)
    {
        RoundTimer toCreate = new RoundTimer(rounds, updateCallback, endCallback);

        if (CurrentRoundMode == RoundMode.RealTime)
        {
            toCreate.StartTimer();
        }

        activeTimers.Add(toCreate);

        return toCreate;
    }

    public void RemoveTimer(RoundTimer roundTimer)
    {
        activeTimers.Remove(roundTimer);
    }

    public void AddHandler(RVN_ComponentHandler toAdd)
    {
        if(!activeHandlers.Contains(toAdd))
        {
            activeHandlers.Add(toAdd);
        }
    }

    public void RemoveHandler(RVN_ComponentHandler toRemove)
    {
        if (activeHandlers.Contains(toRemove))
        {
            activeHandlers.Remove(toRemove);
        }
    }

    public void SetPause(bool toSet)
    {
        if (isPaused != toSet)
        {
            isPaused = toSet;

            if (isPaused)
            {
                foreach (RoundTimer timer in activeTimers)
                {
                    timer.StopTimer();
                }
            }
            else if(currentRoundMode == RoundMode.RealTime)
            {
                if (realTimeGeneralRoundTimer == null)
                {
                    realTimeGeneralRoundTimer = CreateTimer(1f, null, OnGlobalRoundTimerEnd);
                }

                foreach (RoundTimer timer in activeTimers)
                {
                    timer.StartTimer();
                }
            }
        }
    }

    public void SetRealTimeMode()
    {
        if (currentRoundMode != RoundMode.RealTime)
        {
            currentRoundMode = RoundMode.RealTime;

            if (!isPaused)
            {
                if(realTimeGeneralRoundTimer == null)
                {
                    realTimeGeneralRoundTimer = CreateTimer(1f, null, OnGlobalRoundTimerEnd);
                }

                foreach (RoundTimer timer in activeTimers)
                {
                    timer.StartTimer();
                }
            }

            actOnUpdateRoundMode?.Invoke(currentRoundMode);
        }
    }

    public void SetTurnMode()
    {
        if (currentRoundMode != RoundMode.Round)
        {
            currentRoundMode = RoundMode.Round;

            Debug.Log(currentRoundMode);

            if (realTimeGeneralRoundTimer != null)
            {
                RemoveTimer(realTimeGeneralRoundTimer);

                realTimeGeneralRoundTimer.StopTimer();
                realTimeGeneralRoundTimer = null;
            }

            foreach (RoundTimer timer in activeTimers)
            {
                timer.StopTimer();
            }

            actOnUpdateRoundMode?.Invoke(currentRoundMode);
        }
    }

    public void OnEndBattle()
    {
        EndGlobalRound();
        StartGlobalRound();
        SetRealTimeMode();
    }

    public void StartGlobalRound()
    {
        foreach (RVN_ComponentHandler handler in activeHandlers)
        {
            handler.StartRound();
        }
    }

    public void EndGlobalRound()
    {
        foreach (RVN_ComponentHandler handler in activeHandlers)
        {
            handler.EndRound();
        }
    }

    public void OnGlobalRoundTimerEnd()
    {
        //Update Fin Tour Global (Team?/Personnage?)
        EndGlobalRound();

        if (!isPaused)
        {
            realTimeGeneralRoundTimer = CreateTimer(1f, null, OnGlobalRoundTimerEnd);
        }

        //Update Début Tour Global (Team?/Personnage?)
        StartGlobalRound();
    }

    /*private void EndRealTimeTimer()
    {
        realTimeTimer = null;
        EndEveryoneTurn();
        StartEveryoneTurn();
        realTimeTimer = TimerManager.CreateGameTimer(realTimeRoundDuration, EndRealTimeTimer);
    }

    private void EndEveryoneTurn()
    {
        foreach (CombatTeam team in teams)
        {
            foreach (CPN_Character chara in team.characters)
            {
                chara.EndSelfTurn();
                chara.EndTeamTurn();
            }
        }
    }

    private void StartEveryoneTurn()
    {
        foreach (CombatTeam team in teams)
        {
            foreach (CPN_Character chara in team.characters)
            {
                chara.StartTurn();
            }
        }

        StartNewRound();
    }*/
}
