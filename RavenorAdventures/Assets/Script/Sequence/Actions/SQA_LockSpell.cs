using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_LockSpell : SequenceAction
{
    [SerializeField] private bool isLocked;
    [SerializeField] private int spellIndex;
    [SerializeField] private CPN_SpellCaster caster;

    protected override void OnStartAction()
    {
        caster.Spells[spellIndex].LockSpell(isLocked);
        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
