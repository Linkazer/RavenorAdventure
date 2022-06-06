using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_ComponentHandler
{
    [SerializeField] private CharacterScriptable_Battle scriptable;

    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable_Battle> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;
    [SerializeField] private UnityEvent OnStartTurn;
    [SerializeField] private UnityEvent OnEndTurn;


    public Action<RVN_ComponentHandler> ActOnBeginTurn;
    public Action<RVN_ComponentHandler> ActOnEndTurn;

    public bool IsSet => gameObject.activeSelf;

    public CharacterScriptable_Battle Scriptable => scriptable;

    [ContextMenu("Set Character")]
    public void SetCharacter()
    {
        SetCharacterNonCopy(scriptable);
    }

    public void SetCharacter(CharacterScriptable_Battle nScriptable)
    {
        scriptable = Instantiate(nScriptable);

        gameObject.name = scriptable.Nom;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }
    public void SetCharacterNonCopy(CharacterScriptable_Battle nScriptable)
    {
        scriptable = nScriptable;

        gameObject.name = scriptable.Nom;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }



    public void UnsetCharacter(float unsetDelay)
    {
        scriptable = null;

        Debug.Log("Unset Character");
        OnUnsetCharacter?.Invoke();

        if (unsetDelay > 0)
        {
            TimerManager.CreateGameTimer(unsetDelay, () => gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void StartTurn()
    {
        ActOnBeginTurn?.Invoke(this);
        OnStartTurn?.Invoke();

        for(int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetData();
        }
    }

    public void EndTurn()
    {
        ActOnEndTurn?.Invoke(this);
        OnEndTurn?.Invoke();
    }
}
