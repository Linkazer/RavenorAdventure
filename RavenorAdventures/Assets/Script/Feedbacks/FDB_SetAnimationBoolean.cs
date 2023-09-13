using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set a boolean parameter of an animator.
/// </summary>
public class FDB_SetAnimationBoolean : MonoBehaviour, IRVN_FeedbackAction
{
    [SerializeField] private Animator animator;

    [SerializeField] private string animatorParameter;

    [SerializeField] private bool wantedState;

    public void Play()
    {
        animator.SetBool(animatorParameter, wantedState);
    }

    public void Play(bool nState)
    {
        wantedState = nState;
        Play();
    }
}
