using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RVN_Component : MonoBehaviour
{

}

public abstract class RVN_Component<T> : RVN_Component
{
    /// <summary>
    /// Set the data.
    /// </summary>
    /// <param name="toSet">The values of the data to set.</param>
    public abstract void SetData(T toSet);
}
