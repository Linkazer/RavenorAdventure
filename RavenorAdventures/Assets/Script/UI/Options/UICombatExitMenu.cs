using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICombatExitMenu : MonoBehaviour
{
    public void RestartBattle()
    {
        RVN_SceneManager.LoadBattle(RVN_SceneManager.CurrentLevel);
    }
}
