using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LvlEnd_Interaction : RVN_LevelEnd
{
    [SerializeField] private bool looseOnAchieve = false;

    protected override void OnSetLevelEnd()
    {
        
    }

    protected override void OnUnsetLevelEnd()
    {
        
    }

    public void UE_TriggerEnd()
    {
        if (!looseOnAchieve)
        {
            WinLevel();
        }
        else
        {
            LoseLevel();
        }
    }
}
