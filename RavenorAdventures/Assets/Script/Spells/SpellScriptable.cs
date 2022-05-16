using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell/Create Spell")]
public abstract class SpellScriptable : ScriptableObject
{
    [Header("Général Informations")]
    [SerializeField] protected string name;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string description;

    [Header("Animations")]
    [SerializeField] protected CharacterAnimationType castingAnimation;
    [SerializeField] protected float castAnimationDuration;
    [SerializeField] protected InstantiatedAnimationHandler spellAnimation;

    [Header("Comportement")]
    //[SerializeField] protected int cooldown;

    [Header("Forme")]
    [SerializeField] protected int range;
    [SerializeField] protected int zoneRange;
    //[SerializeField] protected List<Vector2Int> zone;
    //[SerializeField] protected bool zoneFaceCaster;
    //[SerializeField] protected bool needVision;

    public string Name => name;
    public Sprite Icon => icon;
    public string Description => description;

    public CharacterAnimationType CastingAnimation => castingAnimation;

    public float CastDuration => castAnimationDuration;

    public InstantiatedAnimationHandler SpellAnimation => spellAnimation;

    public int Range => range;
    public int ZoneRange => zoneRange;
}
