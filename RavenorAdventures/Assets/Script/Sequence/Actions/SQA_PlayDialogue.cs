using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PlayDialogue : SequenceAction
{
    [SerializeField] private DialogueScriptable dialogueToPlay;

    protected override void OnStartAction()
    {
        RVN_DialogueManager.PlayDialogue(dialogueToPlay, EndAction);
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
