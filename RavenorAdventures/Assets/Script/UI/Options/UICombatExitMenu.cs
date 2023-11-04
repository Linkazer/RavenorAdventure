using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICombatExitMenu : MonoBehaviour
{
    public void RestartBattle()
    {
        RVN_SceneManager.LoadCurrentBattle();
    }
}
