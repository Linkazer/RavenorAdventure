using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPN_AnimationHandler : RVN_Component<Animator>
{
    [SerializeField] protected Animator animator;

    /// <summary>
    /// Store the animator of the object.
    /// </summary>
    /// <param name="toSet">The animator to store.</param>
    public override void SetData(Animator toSet)
    {
        animator = toSet;
    }

    /// <summary>
    /// Set a boolean parameter in the animator.
    /// </summary>
    /// <param name="parameter">The name of the parameter.</param>
    /// <param name="value">The value to set on the parameter.</param>
    protected void AnimSetBool(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    /// <summary>
    /// Set a trigger parameter in the animator.
    /// </summary>
    /// <param name="parameter">The name of the parameter.</param>
    protected void AnimSetTrigger(string parameter)
    {
        animator.SetTrigger(parameter);
    }
}
