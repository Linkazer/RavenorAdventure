using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_ComponentHandler : MonoBehaviour
{
    [SerializeField] private List<RVN_Component> components;

    public void GetComponentOfType<T>(ref T wantedComponent) where T : RVN_Component
    {
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] as T != null)
            {
                wantedComponent = components[i] as T;
                break;
            }
        }
    }
}
