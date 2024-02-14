using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RVN_RoundManager;

public class RVN_ComponentHandler : MonoBehaviour
{
    [SerializeField] protected List<RVN_Component> components;

    [SerializeField] public CPN_ANIM_Character animationController; //Code review : A mettre dans les Components ?

    private Dictionary<Type, RVN_Component> componentByType = new Dictionary<Type, RVN_Component>();

    public Node CurrentNode => Grid.GetNodeFromWorldPoint(transform.position);

    /// <summary>
    /// Search for a component of type T.
    /// </summary>
    /// <typeparam name="T">The type of the component we search for.</typeparam>
    /// <param name="wantedComponent">A reference to the component if one is found.</param>
    /// <returns>TRUE if the method find a component of type T.</returns>
    public bool GetComponentOfType<T>(out T wantedComponent) where T : RVN_Component
    {
        wantedComponent = null;

        if(componentByType.ContainsKey(typeof(T)))
        {
            wantedComponent = componentByType[typeof(T)] as T;
            return true;
        }

        return false;

        /*for (int i = 0; i < components.Count; i++)
        {
            if (components[i] as T != null)
            {
                wantedComponent = components[i] as T;
                return true;
            }
        }

        return false;*/
    }

    protected virtual void Start()
    {
        SetHandler();
        Activate();
    }

    public void SetHandler()
    {
        foreach(RVN_Component component in components)
        {
            if(!componentByType.ContainsKey(component.GetType()))
            {
                componentByType.Add(component.GetType(), component);
            }
            else
            {
                Debug.Log($"!!! Component of type {component.GetType()} exist multiple times in {this} !!!");
            }

            component.SetComponent(this);
        }
    }

    public void Activate()
    {
        RVN_RoundManager.Instance.actOnUpdateRoundMode += OnUpdateRoundMode;

        foreach (RVN_Component component in components)
        {
            component.Activate();
        }
    }

    public void Disactivate()
    {
        foreach (RVN_Component component in components)
        {
            component.Disactivate();
        }

        RVN_RoundManager.Instance.actOnUpdateRoundMode -= OnUpdateRoundMode;
    }

    public virtual void OnUpdateRoundMode(RoundMode settedRoundMode)
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnUpdateRoundMode(settedRoundMode);
        }
    }

    /// <summary>
    /// Début d'un nouveau Round
    /// </summary>
    public void StartRound()
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnStartRound();
        }
    }
    /// <summary>
    /// Fin d'un Round complet
    /// </summary>
    public void EndRound()
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnEndRound();
        }
    }
}
