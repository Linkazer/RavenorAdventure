using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_Component
{
    [SerializeField] private RVN_ComponentHandler handler;

    [SerializeField] private UnityEvent OnMouseClic;

    [SerializeField] private List<CPN_CharacterAction> actions;

    public RVN_ComponentHandler Handler => handler;


    private void OnMouseDown() //CODE REVIEW : Le mettre dans un Clic Handler
    {
        OnMouseClic?.Invoke();
    }

    public void StartTurn() //CODE REVIEW : A renommer.
    {
        for(int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetActionData();
        }
    }
}
