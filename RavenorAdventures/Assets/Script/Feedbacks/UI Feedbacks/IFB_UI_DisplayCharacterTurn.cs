using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IFB_UI_DisplayCharacterTurn : MonoBehaviour
{
    [SerializeField] private Animator toActivate;

    [SerializeField] private TextMeshProUGUI text;

    public void Play(CPN_Character character)
    {
        text.text = character.gameObject.name;

        toActivate.SetTrigger("Show");
    }
}
