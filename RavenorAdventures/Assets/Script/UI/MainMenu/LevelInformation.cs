using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelInformation : ScriptableObject
{
    public RVN_LevelManager levelPrefab;
    public string ID;
    public string nom;
    //public string winCondition;
    //public string looseCondition;
    [TextArea(2,4)]
    public string description;
    public List<CharacterScriptable_Battle> charactersInLevel;
}
