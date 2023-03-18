using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RVN_LevelEnd : MonoBehaviour
{
    [SerializeField] private UnityEvent OnWinBattle;
    [SerializeField] private UnityEvent OnLoseBattle;

    private void Start()
    {
        RVN_BattleManager.Instance.OnPlayerTeamDie += LoseLevel;

        OnSetLevelEnd();
    }

    private void UnsetLevelEnd()
    {
        if (RVN_BattleManager.Instance != null)
        {
            RVN_BattleManager.Instance.OnPlayerTeamDie -= LoseLevel;
        }

        OnUnsetLevelEnd();
    }

    protected abstract void OnSetLevelEnd();
    protected abstract void OnUnsetLevelEnd();

    protected void WinLevel()
    {
        UnsetLevelEnd();

        OnWinBattle?.Invoke();

        RVN_BattleManager.Instance.EndLevel(true);
    }

    protected void LoseLevel()
    {
        UnsetLevelEnd();

        OnLoseBattle?.Invoke();

        RVN_BattleManager.Instance.EndLevel(false);
    }
}
