using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RVN_RoundManager;

public abstract class RVN_Component : MonoBehaviour
{
    [SerializeField] protected RVN_ComponentHandler handler;
    public RVN_ComponentHandler Handler => handler;

    /// <summary>
    /// Set the Component
    /// </summary>
    /// <param name="handler"></param>
    public abstract void SetComponent(RVN_ComponentHandler handler);

    public abstract void Activate();
    public abstract void Disactivate();

    public virtual void OnUpdateRoundMode(RoundMode settedMode)
    {

    }

    /// <summary>
    /// Début d'un nouveau Round
    /// </summary>
    public virtual void OnStartRound()
    {

    }
    /// <summary>
    /// Fin d'un Round complet
    /// </summary>
    public virtual void OnEndRound()
    {

    }
    /// <summary>
    /// Début du tour de l'Handler (Character)
    /// </summary>
    public virtual void OnStartHandlerRound()
    {

    }
    /// <summary>
    /// Fin du tour de l'Handler (Character)
    /// </summary>
    public virtual void OnEndHandlerRound()
    {

    }
    /// <summary>
    /// Début du tour du Group (Team)
    /// </summary>
    public virtual void OnStartHandlerGroupRound()
    {

    }
    /// <summary>
    /// Fin du tour du Group (Team)
    /// </summary>
    public virtual void OnEndHandlerGroupRound()
    {

    }
}

public abstract class RVN_Component<T> : RVN_Component
{
    public override void SetComponent(RVN_ComponentHandler handler)
    {
        T data = GetDataFromHandler();

        if (data != null)
        {
            SetData(data);
        }
    }

    protected abstract T GetDataFromHandler();

    /// <summary>
    /// Set the data.
    /// </summary>
    /// <param name="toSet">The values of the data to set.</param>
    protected abstract void SetData(T toSet);
}
