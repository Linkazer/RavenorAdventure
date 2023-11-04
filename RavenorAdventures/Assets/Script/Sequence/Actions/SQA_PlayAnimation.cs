using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PlayAnimation : SequenceAction
{
    [SerializeField] private CPN_ANIM_Character animatedObject;
    [SerializeField] private string animationToPlay;

    protected override void OnStartAction()
    {
        animatedObject.PlayAnimation(animationToPlay);
        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
