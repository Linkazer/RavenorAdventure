using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ActivateObject : SequenceAction
{
    [SerializeField] private GameObject toActivate;
    [SerializeField] private bool toSet;

    protected override void OnStartAction()
    {
        toActivate.SetActive(toSet);
        EndAction();
    }

    protected override void OnEndAction()
    {
       
    }

    protected override void OnSkipAction()
    {

    }
}
