using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_Component
{
    [SerializeField] private CharacterScriptable scriptable;


    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    public Action<RVN_ComponentHandler> ActOnBeginTurn;
    public Action<RVN_ComponentHandler> ActOnEndTurn;

    public bool IsSet => gameObject.activeSelf;

    [ContextMenu("Set Character")]
    public void SetCharacter()
    {
        SetCharacter(scriptable);
    }

    public void SetCharacter(CharacterScriptable nScriptable)
    {
        scriptable = nScriptable;

        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }

    public void UnsetCharacter()
    {
        scriptable = null;

        OnUnsetCharacter?.Invoke();

        gameObject.SetActive(false);
    }

    public void StartTurn()
    {
        ActOnBeginTurn?.Invoke(Handler);

        for(int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetData();
        }
    }

    public void EndTurn()
    {
        ActOnEndTurn?.Invoke(Handler);
    }
}
