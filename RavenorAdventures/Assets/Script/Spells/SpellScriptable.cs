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

public enum SpellCastType
{
    Normal,
    Fast,
}

public enum SpellAnimationTarget
{
    All,
    Target,
    HitedCharacters,
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
    [SerializeField, Tooltip("Does the SpellAnimation only play on the main target of the spell")] protected SpellAnimationTarget spellAnimationTarget = SpellAnimationTarget.All;
    [SerializeField] protected SpellProjectile projectile;
    [SerializeField] protected InstantiatedAnimationHandler spellAnimation;
    [SerializeField] protected float animationDuration;

    [Header("Ressources")]
    [SerializeField] protected int ressourceCost;
    [SerializeField] protected int maxUtilisations = -1;
    protected int utilisationLeft;

    [Header("Cast Data")]
    [SerializeField] protected SpellTargets hitableTarget = SpellTargets.All;
    [SerializeField] protected SpellTargets castTarget = SpellTargets.All;
    [SerializeField] protected SpellCastType castType = SpellCastType.Normal;

    [Header("Cooldowns")]
    [SerializeField] protected bool isCooldownGlobal = false;
    [SerializeField] protected int cooldown;
    protected int currentCooldown;

    [Header("Forme")]
    [SerializeField] protected int range;
    [SerializeField] protected int zoneRange;
    //[SerializeField] protected List<Vector2Int> zone;
    //[SerializeField] protected bool zoneFaceCaster;
    //[SerializeField] protected bool needVision;

    [Header("Base Effects")]
    [SerializeField] private List<EffectScriptable> effectsOnTarget;
    [SerializeField] private List<EffectScriptable> effectsOnCaster;


    public Action<int> OnUpdateCooldown;
    public Action<int> OnUpdateUtilisationLeft;

    public string Name => nom;
    public Sprite Icon => icon;
    public string Description => description.GetText();

    public CharacterAnimationType CastingAnimation => castingAnimation;

    public float CastDuration => castAnimationDuration;

    public SpellAnimationTarget AnimationTarget => spellAnimationTarget;
    public SpellProjectile Projectile => projectile;
    public InstantiatedAnimationHandler SpellAnimation => spellAnimation;
    public float AnimationDuration => animationDuration;

    public int RessourceCost => ressourceCost;
    public int MaxUtilisation => maxUtilisations;
    public int UtilisationLeft => utilisationLeft;

    public SpellTargets HitableTargets => hitableTarget;
    public SpellTargets CastTargets => castTarget;
    public SpellCastType CastType => castType;
    public int StartCooldown => cooldown;

    public bool IsCooldownGlobal => isCooldownGlobal;
    public int CurrentCooldown => currentCooldown;

    public int Range => range;
    public int ZoneRange => zoneRange;

    public bool IsUsable => currentCooldown <= 0 && (maxUtilisations <= 0 || utilisationLeft > 0);

    public List<EffectScriptable> Effects()
    {
        return new List<EffectScriptable>(effectsOnTarget);
    }

    public List<EffectScriptable> EffectOnCaster => effectsOnCaster;

    public abstract void SetCaster(CPN_SpellCaster caster);

    public void SetSpell()
    {
        utilisationLeft = maxUtilisations;
        OnUpdateUtilisationLeft?.Invoke(utilisationLeft);
    }

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
        SetCooldown(cooldown);
    }

    public void SetCooldown(int valueToSet)
    {
        currentCooldown = valueToSet;
        OnUpdateCooldown?.Invoke(currentCooldown);
    }

    public void UseSpell()
    {
        utilisationLeft--;
        OnUpdateUtilisationLeft?.Invoke(utilisationLeft);
    }
}
