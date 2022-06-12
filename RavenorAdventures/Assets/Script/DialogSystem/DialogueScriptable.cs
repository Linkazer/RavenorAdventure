using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField] private CharacterScriptable leftCharacter;
    [SerializeField] private CharacterScriptable rightCharacter;
    [SerializeField] private CharacterScriptable talker;

    [SerializeField] private Sprite background;

    [SerializeField] private RVN_Text text;
    [SerializeField] private List<DialogueResponse> responses;

    public CharacterScriptable LeftCharacter => leftCharacter;
    public CharacterScriptable RightCharacter => rightCharacter;
    public CharacterScriptable Talker => talker;

    public Sprite Background => background;

    public RVN_Text Text => text;

    public List<DialogueResponse> Responses => responses;
}
