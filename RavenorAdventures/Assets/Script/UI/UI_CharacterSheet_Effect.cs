using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSheet_Effect : MonoBehaviour
{
    [SerializeField] private Image effectImage;

    public void SetEffect(EffectScriptable effect)
    {
        effectImage.sprite = effect.Icon;
    }
}
