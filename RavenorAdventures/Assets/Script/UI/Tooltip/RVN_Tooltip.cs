using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UI/Tooltip", fileName ="Tooltip")]
public class RVN_Tooltip : ScriptableObject
{
    [SerializeField] private RVN_Text text;

    public string Text => text.GetText();
}
