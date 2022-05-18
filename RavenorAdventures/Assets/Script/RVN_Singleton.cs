using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_Singleton<T> : MonoBehaviour where T : RVN_Singleton<T>
{
    protected static T instance = null;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this as T;
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
