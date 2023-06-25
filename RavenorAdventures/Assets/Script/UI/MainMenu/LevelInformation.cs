using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelInformation : ScriptableObject
{
    public RVN_LevelManager levelPrefab;
    public string ID;

    [SerializeField] private RVN_Text levelName;
    [SerializeField] private RVN_Text levelDescription;

    public List<CharacterScriptable_Battle> charactersInLevel;

    public string Nom => levelName.GetText();

    public string Description => levelDescription.GetText();
}
