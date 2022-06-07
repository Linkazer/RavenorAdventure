using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSheet : RVN_Singleton<UI_CharacterSheet>
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterIcon;

    [SerializeField] private List<TextMeshProUGUI> stats = new List<TextMeshProUGUI>();

    [SerializeField] private List<Image> effects;

    private CPN_Character currentCharacter;

    protected override void OnAwake()
    {
        UnsetCharacter();
    }

    public static void TrySetCharacter(CPN_ClicHandler touchedObject)
    {
        if(touchedObject.Handler.GetComponentOfType<CPN_Character>(out CPN_Character character))
        {
            Debug.Log(instance);
            instance.SetCharacter(character);
        }
        else
        {
            instance.UnsetCharacter();
        }
    }

    public void SetCharacter(CPN_Character nCharacter)
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            gameObject.SetActive(true);
        }

        currentCharacter = nCharacter;

        CharacterScriptable_Battle charaScriptable = currentCharacter.Scriptable;

        characterName.text = charaScriptable.Nom;
        characterIcon.sprite = charaScriptable.Portrait;

        stats[0].text = charaScriptable.Power().ToString();
        stats[1].text = charaScriptable.Defense().ToString();
        stats[2].text = charaScriptable.Accuracy().ToString();

        if(nCharacter.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
        {
            for(int i = 0; i < effectHandler.Effects.Count; i++)
            {
                effects[i].sprite = effectHandler.Effects[i].GetEffect.Icon;
                effects[i].gameObject.SetActive(true);
            }
        }
    }

    public void UnsetCharacter()
    {
        foreach (Image effect in effects)
        {
            effect.gameObject.SetActive(false);
        }

        currentCharacter = null;


        gameObject.SetActive(false);
    }
}
