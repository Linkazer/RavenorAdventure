using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/Dialogue Character")]
public class CharacterScriptable : ScriptableObject
{
    [SerializeField] protected RVN_Text nom;
    [SerializeField] protected RVN_Text description;
    [SerializeField] protected Sprite UIPortrait;
    [SerializeField] protected Color nameColor = Color.black;

    public string Nom => nom.GetText();
    public string Description => description.GetText();
    public Color NameColor => nameColor;
    public Sprite Portrait => UIPortrait;
}
