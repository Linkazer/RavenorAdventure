using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVNUI_LoadBattle : MonoBehaviour
{
    public void ReloadBattle()
    {
        RVN_BattleManager.Instance.RetryBattle();
    }

    public void LoadNextBattle()
    {
        RVN_BattleManager.Instance.LoadNextBattle();
    }
}
