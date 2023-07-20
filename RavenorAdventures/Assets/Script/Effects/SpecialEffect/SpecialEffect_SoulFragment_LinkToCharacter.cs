using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect_SoulFragment_LinkToCharacter : MonoBehaviour
{
    [SerializeField] private EffectScriptable effectToCheck;

    private CPN_Character linkedCharacter;

    private void LateUpdate()
    {
        SearchForCharacterToLink();
        enabled = false;
    }

    public void SearchForCharacterToLink()
    {
        foreach(CPN_Character chara in RVN_BattleManager.GetPlayerTeam)
        {
            if(chara.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
            {
                if (effectHandler.HasEffect(effectToCheck) && !SpecialEffect_SoulFragment_CharacterLinkManager.Instance.IsCharacterLinked(chara, effectToCheck))
                {
                    linkedCharacter = chara;
                    SpecialEffect_SoulFragment_CharacterLinkManager.Instance.AddCharacter(chara, effectToCheck);
                }
            }
        }
    }

    public void RemoveCharacterLink()
    {
        if (linkedCharacter.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
        {
            effectHandler.RemoveEffect(effectToCheck);
        }

        SpecialEffect_SoulFragment_CharacterLinkManager.Instance.RemoveCharacter(linkedCharacter, effectToCheck);
        linkedCharacter = null;
    }
}
