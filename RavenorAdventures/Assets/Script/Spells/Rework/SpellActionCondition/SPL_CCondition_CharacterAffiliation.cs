using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_CCondition_CharacterAffiliation : SPL_SpellActionChooserCondition
{
    [Serializable]
    private enum PossibleAffiliation
    {
        Ally,
        Ennemy,
    }

    [SerializeField] private PossibleAffiliation affiliationWanted;

    public override bool IsConditionValid(SPL_CastedSpell castedSpellData)
    {
        List<CPN_Character> targetCharacters = castedSpellData.TargetNode.GetNodeHandler<CPN_Character>();

        CPN_Character casterCharacter = castedSpellData.Caster.Handler as CPN_Character;

        if (casterCharacter != null && targetCharacters.Count > 0)
        {
            switch (affiliationWanted)
            {
                case PossibleAffiliation.Ally:
                    return RVN_BattleManager.AreCharacterAllies(casterCharacter, targetCharacters[0]);
                case PossibleAffiliation.Ennemy:
                    return !RVN_BattleManager.AreCharacterAllies(casterCharacter, targetCharacters[0]);
            }
        }
        else 
        {
            return false;
        }

        return true;
    }
}
