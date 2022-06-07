using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellTargets
{
    Allies,
    Ennemies,
    Self,
    All
}

public abstract class SpellScriptable : ScriptableObject, CPN_Data_EffectHandler
{
    [Header("Général Informations")]
    [SerializeField] protected string nom;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected RVN_Text description;

    [Header("Animations")]
    [SerializeField] protected CharacterAnimationType castingAnimation;
    [SerializeField] protected float castAnimationDuration;
    [SerializeField] protected InstantiatedAnimationHandler spellAnimation;

    [Header("Comportement")]
    [SerializeField] protected SpellTargets hitableTarget = SpellTargets.All;
    [SerializeField] protected SpellTargets castTarget = SpellTargets.All;
    [SerializeField] protected int cooldown;
    [SerializeField] protected int currentCooldown;

    [Header("Forme")]
    [SerializeField] protected int range;
    [SerializeField] protected int zoneRange;
    //[SerializeField] protected List<Vector2Int> zone;
    //[SerializeField] protected bool zoneFaceCaster;
    //[SerializeField] protected bool needVision;

    [Header("Effects")]
    [SerializeField] private List<EffectScriptable> effects;


    public Action<int> OnUpdateCooldown;

    public string Name => nom;
    public Sprite Icon => icon;
    public string Description => description.Text();

    public CharacterAnimationType CastingAnimation => castingAnimation;

    public float CastDuration => castAnimationDuration;

    public InstantiatedAnimationHandler SpellAnimation => spellAnimation;

    public SpellTargets HitableTargets => hitableTarget;
    public SpellTargets CastTargets => castTarget;
    public int StartCooldown => cooldown;
    public int CurrentCooldown => currentCooldown;

    public int Range => range;
    public int ZoneRange => zoneRange;

    public bool IsUsable => currentCooldown <= 0;

    public List<EffectScriptable> Effects()
    {
        return new List<EffectScriptable>(effects);
    }

    public abstract void SetCaster(CPN_SpellCaster caster);

    public void UpdateCurrentCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;

            OnUpdateCooldown?.Invoke(currentCooldown);
        }
    }

    public void ResetCooldown()
    {
        currentCooldown = cooldown;
        OnUpdateCooldown?.Invoke(currentCooldown);
    }
}
