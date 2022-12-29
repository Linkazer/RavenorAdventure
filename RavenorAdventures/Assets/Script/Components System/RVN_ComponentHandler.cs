using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_ComponentHandler : RVN_Component
{
    [SerializeField] private List<RVN_Component> components;

    [SerializeField] public CPN_ANIM_Character animationController;

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

        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] as T != null)
            {
                wantedComponent = components[i] as T;
                return true;
            }
        }

        return false;
    }
}
