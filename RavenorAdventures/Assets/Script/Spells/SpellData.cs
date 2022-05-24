using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellData
{
    [Header("Général Informations")]
    [SerializeField] protected string name;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string description;

    [Header("Animation")]
    [SerializeField] protected CharacterAnimationType castingAnimation;
    [SerializeField] protected float castAnimationDuration;
    [SerializeField] protected InstantiatedAnimationHandler spellAnimation;

    [Header("Forme")]
    [SerializeField] protected int range;
    [SerializeField] protected int zoneRange;
}
