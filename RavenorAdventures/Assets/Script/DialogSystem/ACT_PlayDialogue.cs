using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACT_PlayDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;

    public void PlayDialogue()
    {
        RVN_DialogueManager.PlayDialogue(dialogue);
    }

    public void PlayDialogue(DialogueScriptable toPlay)
    {
        RVN_DialogueManager.PlayDialogue(toPlay);
    }
}
