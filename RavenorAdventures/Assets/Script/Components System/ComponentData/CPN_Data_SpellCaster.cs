using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_SpellCaster
{
    public List<SpellScriptable> AvailableSpells();
    public int MaxSpellUseByTurn();
    public int PossibleRelance();
    public int Accuracy();
    public int Power();
}
