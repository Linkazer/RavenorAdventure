using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelInformation : ScriptableObject
{
    [SerializeField] private RVN_LevelManager levelPrefab;
    [SerializeField] private string ID;

    [SerializeField] private RVN_Text levelName;
    [SerializeField] private RVN_Text levelDescription;

    [SerializeField] private LevelInformation nextLevel;

    public List<CharacterScriptable_Battle> charactersInLevel;

    public string Nom => levelName.GetText();

    public string Description => levelDescription.GetText();

    public RVN_LevelManager Prefab => levelPrefab;

    public string GetID => ID;

    public LevelInformation NextLevel => nextLevel;
}
