using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect_InflictDamageOnDeath : MonoBehaviour
{
    [SerializeField] private CharacterScriptable_Battle target;
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

        foreach(CPN_Character chara in RVN_BattleManager.GetAllCharacter())
        {
            if(chara.Scriptable.name.Contains(target.name))
            {
                launchedSpell = new LaunchedSpellData(spellToUse, null, chara.CurrentNode);

                RVN_SpellManager.UseSpell(launchedSpell, null);
            }
        }

        characterHealthToCheck.actOnDeath -= CheckDeath;
    }
}
