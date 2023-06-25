using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu_LevelSelectionManager : MonoBehaviour
{
    private LevelInformation selectedLevel;

    [SerializeField] private CanvasGroup levelInformationGroup;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelDescription;

    [SerializeField] private MainMenuCharacterChoice[] characters;

    public void SelectLevel(LevelInformation newLevel)
    {
        if(selectedLevel == newLevel)
        {
            return;
        }

        selectedLevel = newLevel;

        levelName.text = selectedLevel.Nom;
        levelDescription.text = selectedLevel.Description;

        for(int i = 0; i < characters.Length; i++)
        {
            if(i < selectedLevel.charactersInLevel.Count)
            {
                characters[i].SetCharacter(selectedLevel.charactersInLevel[i]);
            }
            else
            {
                characters[i].gameObject.SetActive(false);
            }
        }

        levelInformationGroup.alpha = 1;
    }

    public void LoadLevel()
    {
        RVN_SceneManager.LoadBattle(selectedLevel);
    }
}
