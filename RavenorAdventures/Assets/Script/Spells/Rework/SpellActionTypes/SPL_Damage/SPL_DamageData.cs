using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_DamageData
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_DamageOrigin))] private SPL_DamageOrigin origin;
    [SerializeField] private SPL_DamageType damageType;

    [SerializeField] private bool needHit = true;

    public bool NeedHit => needHit;

    public SPL_DamageOrigin Origin => origin;
    public SPL_DamageType DamageType => damageType;
}
