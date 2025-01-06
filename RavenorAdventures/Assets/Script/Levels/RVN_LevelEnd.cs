using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RVN_LevelEnd : MonoBehaviour
{
    [SerializeField] private UnityEvent OnWinBattle;
    [SerializeField] private UnityEvent OnLoseBattle;

    public void SetLevelEnd()
    {
        OnSetLevelEnd();
    }

    private void UnsetLevelEnd()
    {
        RVN_BattleManager.OnPlayerTeamDie -= LoseLevel;

        OnUnsetLevelEnd();
    }

    protected abstract void OnSetLevelEnd();
    protected abstract void OnUnsetLevelEnd();

    protected void WinLevel()
    {
        UnsetLevelEnd();

        OnWinBattle?.Invoke();

        RVN_BattleManager.Instance.EndBattle(true);
    }

    protected void LoseLevel()
    {
        UnsetLevelEnd();

        OnLoseBattle?.Invoke();

        RVN_BattleManager.Instance.EndBattle(false);
    }
}
