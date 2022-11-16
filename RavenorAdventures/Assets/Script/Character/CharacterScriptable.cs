using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/Dialogue Character")]
public class CharacterScriptable : ScriptableObject
{
    [SerializeField] protected string nom;
    [SerializeField] protected Sprite UIPortrait;
    [SerializeField] protected Color nameColor = Color.black;

    public string Nom => nom;
    public Color NameColor => nameColor;
    public Sprite Portrait => UIPortrait;
}
