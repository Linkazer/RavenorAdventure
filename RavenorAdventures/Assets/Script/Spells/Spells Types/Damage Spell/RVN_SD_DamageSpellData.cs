using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RVN_SD_DamageSpellData : SpellData
{
    [SerializeField] private int damageDealt;

    public int DamageDealt => damageDealt;

    public override SpellData GetCopy()
    {
        RVN_SD_DamageSpellData toReturn = new RVN_SD_DamageSpellData(); //ToReturn est null

        toReturn.name = name;
        toReturn.icon = icon;
        toReturn.description = description;
        toReturn.zoneRange = zoneRange;

        toReturn.damageDealt = damageDealt;

        return toReturn;
    }
}
