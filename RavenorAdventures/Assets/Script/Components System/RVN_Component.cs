using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RVN_Component : MonoBehaviour
{
    [SerializeField] protected RVN_ComponentHandler handler;
    public RVN_ComponentHandler Handler => handler;

    public abstract void OnEnterBattle();

    public abstract void OnExitBattle();
}

public abstract class RVN_Component<T> : RVN_Component
{
    /// <summary>
    /// Set the data.
    /// </summary>
    /// <param name="toSet">The values of the data to set.</param>
    public abstract void SetData(T toSet);
}
