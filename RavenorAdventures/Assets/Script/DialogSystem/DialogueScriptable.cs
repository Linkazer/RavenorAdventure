using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueSentence
{
    //public CharacterScriptable leftCharacter;
    //public CharacterScriptable rightCharacter;
    public CharacterScriptable talker;

    public RVN_Text text;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField] private DialogueSentence[] sentences;

    [SerializeField] private List<DialogueResponse> responses;

    public DialogueSentence[] Sentences => sentences;

    public List<DialogueResponse> Responses => responses;
}
