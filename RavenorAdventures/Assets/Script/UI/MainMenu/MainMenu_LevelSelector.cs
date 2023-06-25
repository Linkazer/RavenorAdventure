using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu_LevelSelector : MonoBehaviour
{
    [SerializeField] private LevelInformation level;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private MainMenu_LevelSelectionManager selectionManager;

    private void OnEnable()
    {
        levelName.text = level.Nom;
    }

    public void SelectLevel()
    {
        selectionManager.SelectLevel(level);
    }
}
