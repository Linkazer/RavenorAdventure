using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_Component
{
    [SerializeField] private CharacterScriptable scriptable;

    [SerializeField] private RVN_ComponentHandler handler;

    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    public RVN_ComponentHandler Handler => handler;

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

    public void StartTurn() //CODE REVIEW : A renommer.
    {
        for(int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetData();
        }
    }
}
