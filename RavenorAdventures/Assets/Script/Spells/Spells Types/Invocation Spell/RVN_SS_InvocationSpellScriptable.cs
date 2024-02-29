using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Invocation Spell", menuName = "Spell/Create Invocation Spell")]
public class RVN_SS_InvocationSpellScriptable : SpellScriptable
{
    [SerializeField] private CPN_Character toSpawn;

    [SerializeField] private int baseTeam;

    private int currentTeam;

    public CPN_Character ToSpawn => toSpawn;

    public int TeamIndex => currentTeam;

    public override void SetCaster(CPN_SpellCaster caster)
    {
        if(caster != null)
        {
            currentTeam = RVN_BattleManager.GetCharacterTeamIndex(caster.Handler as CPN_Character);
        }
        else
        {
            currentTeam = baseTeam;
        }
    }
}
