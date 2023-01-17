using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCharacterSheet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterDescription;

    [SerializeField] private List<TextMeshProUGUI> stats = new List<TextMeshProUGUI>();

    private CharacterScriptable_Battle currentCharacter;

    public void SetCharacter(CharacterScriptable_Battle nCharacter)
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            gameObject.SetActive(true);
        }

        currentCharacter = nCharacter;

        characterName.text = nCharacter.Nom;
        characterDescription.text = nCharacter.Description;

        stats[1].text = nCharacter.Power().ToString();
        stats[3].text = nCharacter.Accuracy().ToString();


        stats[2].text = nCharacter.Defense().ToString();
        stats[0].text = nCharacter.MaxHealth().ToString();

    }

    public void UnsetCharacter()
    {
        currentCharacter = null;

        gameObject.SetActive(false);
    }
}
