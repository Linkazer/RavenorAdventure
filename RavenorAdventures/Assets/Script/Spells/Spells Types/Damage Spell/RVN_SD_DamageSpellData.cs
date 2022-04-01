using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RVN_SD_DamageSpellData : SpellData
{
    [SerializeField] private int damageDealt;

    public int Test;

    public override SpellData GetCopy()
    {
        RVN_SD_DamageSpellData toReturn = new RVN_SD_DamageSpellData(); //ToReturn est null

        toReturn.name = name;
        toReturn.icon = icon;
        toReturn.description = description;

        toReturn.damageDealt = damageDealt;
        toReturn.Test = Test;

        return toReturn;
    }
}
