using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RVN_Component : MonoBehaviour
{
    
}

public abstract class RVN_Component<T> : RVN_Component
{
    public abstract void SetData(T toSet);
}
