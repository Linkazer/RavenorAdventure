using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character_EffectApplyDisplayer : MonoBehaviour
{
    [SerializeField] private Image effectSprite;
    [SerializeField] private float displayDuration;
    [SerializeField] private Color removeEffectColor;

    private void OnEnable()
    {
        TimerManager.CreateGameTimer(displayDuration, () => Destroy(gameObject));
    }

    public void Display(Sprite effectToDisplay, bool addEffect)
    {
        effectSprite.sprite = effectToDisplay;

        effectSprite.color = addEffect ? Color.white : removeEffectColor;
    }
}
