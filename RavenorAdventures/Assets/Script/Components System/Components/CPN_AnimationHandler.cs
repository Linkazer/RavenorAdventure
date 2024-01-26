using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPN_AnimationHandler : RVN_Component
{
    [SerializeField] protected Animator animator;

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

    public override void SetComponent(RVN_ComponentHandler handler)
    {
        if(animator == null)
        {
            Debug.Log($"!!! Missing animator on {this} !!!");
        }
    }

    public override void Activate()
    {
        
    }

    public override void Disactivate()
    {
        
    }
}
