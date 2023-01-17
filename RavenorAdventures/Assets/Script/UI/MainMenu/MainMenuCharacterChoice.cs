using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCharacterChoice : MonoBehaviour
{
    [SerializeField] private CharacterScriptable_Battle character;
    [SerializeField] private MainMenuCharacterSheet characterSheet;

    [SerializeField] private Image portrait;

    public void SetCharacter(CharacterScriptable_Battle toSet)
    {
        character = toSet;

        portrait.sprite = character.Portrait;
    }

    public void OnClick()
    {
        characterSheet.SetCharacter(character);
    }
}
