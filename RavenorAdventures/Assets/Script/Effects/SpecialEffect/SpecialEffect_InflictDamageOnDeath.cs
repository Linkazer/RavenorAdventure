using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect_InflictDamageOnDeath : MonoBehaviour
{
    [SerializeField] private List<CharacterScriptable_Battle> targets;
    [SerializeField] private SpellScriptable spellToUse;
    [SerializeField] private CPN_HealthHandler characterHealthToCheck;

    private void Start()
    {
        characterHealthToCheck.actOnDeath += CheckDeath;
    }

    private void OnDestroy()
    {
        characterHealthToCheck.actOnDeath -= CheckDeath;
    }

    private void CheckDeath(RVN_ComponentHandler targetHandler)
    {
        LaunchedSpellData launchedSpell = null;

        List<CPN_Character> allCharacters = new List<CPN_Character>(RVN_BattleManager.GetAllCharacter());

        foreach (CharacterScriptable_Battle target in targets)
        {
            foreach (CPN_Character chara in allCharacters)
            {
                if (chara.Scriptable.name.Contains(target.name))
                {
                    launchedSpell = new LaunchedSpellData(spellToUse, null, chara.CurrentNode);

                    RVN_SpellManager.UseSpell(launchedSpell, null);

                    break;
                }
            }
        }

        characterHealthToCheck.actOnDeath -= CheckDeath;
    }
}
