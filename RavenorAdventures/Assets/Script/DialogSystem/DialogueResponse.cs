using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueResponse
{
    [SerializeField] private RVN_Text text;
    [SerializeField] private DialogueScriptable nextDialogue;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(DialogueResponseEffect))] private List<DialogueResponseEffect> effects;

    public RVN_Text Text => text;
    public DialogueScriptable NextDialogue => nextDialogue;
    public List<DialogueResponseEffect> Effects => effects;
}
