using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_Component
{
    [SerializeField] private RVN_ComponentHandler handler;

    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    [SerializeField] private CharacterScriptable scriptable;

    public RVN_ComponentHandler Handler => handler;

    public bool IsSet => gameObject.activeSelf;

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
