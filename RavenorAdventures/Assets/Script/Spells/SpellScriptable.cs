using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public enum SpellTargets
{
    Allies,
    Ennemies,
    Self,
    All
}

public enum SpellZoneType
{
    Normal,
    AllPossibleTarget,
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

[System.Obsolete("Use SPL_SpellScriptable")]
public abstract class SpellScriptable : ScriptableObject, CPN_Data_EffectHandler
{
    [Header("Général Informations")]
    [SerializeField] protected RVN_Text displayName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected RVN_Text description;

    [Header("Animations")]
    [SerializeField] protected CharacterAnimationType castingAnimation;
    [SerializeField] protected CharacterAnimationType launchAnimation;
    [SerializeField] protected float castAnimationDuration;
    [SerializeField, Tooltip("Does the SpellAnimation only play on the main target of the spell")] protected SpellAnimationTarget spellAnimationTarget = SpellAnimationTarget.All;
    [SerializeField] protected SpellProjectile projectile;
    [SerializeField] protected InstantiatedAnimationHandler spellAnimation;
    [SerializeField] protected float animationDuration;

    [Header("Sounds")]
    [SerializeField] protected AudioData launchAudioData;

    [Header("Ressources")]
    [SerializeField] protected int ressourceCost;
    [SerializeField] protected int maxUtilisations = -1;
    [SerializeField] protected bool removeOnNoUutlisation = false;
    protected int utilisationLeft;
    protected bool isSpellLocked;

    [Header("Cast Data")]
    [SerializeField] protected SpellTargets hitableTarget = SpellTargets.All;
    [SerializeField] protected SpellTargets castTarget = SpellTargets.All;
    [SerializeField] protected SpellCastType castType = SpellCastType.Normal;

    [Header("Cooldowns")]
    [SerializeField] protected bool isCooldownGlobal = false;
    [SerializeField] protected int cooldown;
    protected RoundTimer cooldownTimer;

    [Header("Forme")]
    [SerializeField] protected SpellZoneType zoneType = SpellZoneType.Normal;
    [SerializeField] protected int range;
    [SerializeField] protected int zoneRange;
    [SerializeField] protected bool zoneFaceCaster;
    [SerializeField] protected List<Vector2Int> zoneDefined;
    //[SerializeField] protected bool needVision;

    [Header("Base Effects")]
    [SerializeField] private List<EffectScriptable> effectsOnTarget;
    [SerializeField] private List<EffectScriptable> effectsOnCaster;


    public Action<int> OnUpdateCooldown;
    public Action<int> OnUpdateUtilisationLeft;
    public Action<bool> OnLockSpell;

    public string Name => displayName.GetText();
    public Sprite Icon => icon;

    public bool RemoveOnNoUtilisation => removeOnNoUutlisation;

    public virtual string GetDescription()
    {
        string[] subText = description.GetText().Split('%');

        string toReturn = "";

        for(int i = 0; i < subText.Length; i++)
        {
            if(i%2 == 0)
            {
                toReturn += subText[i];
            }
            else
            {
                toReturn += this.GetType().GetField(subText[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this).ToString();
            }
        }

        if (effectsOnTarget.Count > 0)
        {
            foreach (EffectScriptable eff in effectsOnTarget)
            {
                if (!eff.HideOnApply)
                {
                    toReturn += $"\n <b> {eff.Name} </b>  : {eff.Description}";
                }
            }
        }

        if (effectsOnCaster.Count > 0)
        {
            foreach (EffectScriptable eff in effectsOnCaster)
            {
                if (!eff.HideOnApply)
                {
                    toReturn += $"\n <b> {eff.Name} </b>  : {eff.Description}";
                }
            }
        }

        return toReturn;
    }

    public CharacterAnimationType CastingAnimation => castingAnimation;

    public CharacterAnimationType LaunchSpellAnimation => launchAnimation;

    public AudioData AnimationAudioData => launchAudioData;

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
    public int CooldownDuration => cooldown;

    public bool IsCooldownGlobal => isCooldownGlobal;
    public int CurrentCooldown => cooldownTimer != null ? Mathf.CeilToInt(cooldownTimer.roundLeft) : 0;

    public SpellZoneType ZoneType => zoneType;
    public int Range => range;
    public int ZoneRange => zoneRange;

    public bool IsUsable => !isSpellLocked && CurrentCooldown <= 0 && (maxUtilisations <= 0 || utilisationLeft > 0);

    public bool IsLocked => isSpellLocked;

    public List<EffectScriptable> Effects()
    {
        return new List<EffectScriptable>(effectsOnTarget);
    }

    public List<EffectScriptable> EffectOnCaster => effectsOnCaster;

    public abstract void SetCaster(CPN_SpellCaster caster);

    public void SetSpell()
    {
        cooldownTimer = null;
        utilisationLeft = maxUtilisations;
        OnUpdateUtilisationLeft?.Invoke(utilisationLeft);
    }

    public void UpdateCurrentCooldown()
    {
        if (cooldownTimer != null && cooldownTimer.roundLeft > 0)
        {
            if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.Round)
            {
                cooldownTimer.ProgressTimer(1);
            }

            OnUpdateCooldown?.Invoke(CurrentCooldown);
        }
    }

    public void StartCooldown()
    {
        SetCooldown(cooldown);
    }

    public void SetCooldown(int valueToSet)
    {
        cooldownTimer = RVN_RoundManager.Instance.CreateTimer(valueToSet, UpdateCurrentCooldown, EndCooldownTimer);
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }

    private void EndCooldownTimer()
    {
        cooldownTimer = null;
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }

    public void LockSpell(bool toSet)
    {
        isSpellLocked = toSet;
        OnLockSpell?.Invoke(isSpellLocked);
    }

    public void UseSpell()
    {
        utilisationLeft--;
        OnUpdateUtilisationLeft?.Invoke(utilisationLeft);
    }

    public virtual List<Node> GetZone(Node startNode, Node casterNode)
    {
        List<Node> toReturn = new List<Node>();

        if (zoneDefined.Count > 0)
        {
            Vector2 direction = Vector2.one;
            if (zoneFaceCaster && casterNode != null)
            {
                direction = new Vector2(startNode.gridX, startNode.gridY) - new Vector2(casterNode.gridX, casterNode.gridY);
            }

            foreach (Vector2Int v in zoneDefined)
            {
                Node toAdd = null;

                if (direction.y == 0 && direction.x == 0)
                {
                    toAdd = Grid.GetNode(startNode.gridX + v.x, startNode.gridY + v.y);
                }
                else if (direction.y > 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
                {
                    toAdd = Grid.GetNode(startNode.gridX + v.x, startNode.gridY + v.y);
                }
                else if (direction.x < 0 && (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) || direction.x == -direction.y))
                {
                    toAdd = Grid.GetNode(startNode.gridX - v.y, startNode.gridY + v.x);
                }
                else if (direction.y < 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
                {
                    toAdd = Grid.GetNode(startNode.gridX - v.x, startNode.gridY - v.y);
                }
                else
                {
                    toAdd = Grid.GetNode(startNode.gridX + v.y, startNode.gridY - v.x);
                }

                if(toAdd != null)
                {
                    toReturn.Add(toAdd);
                }
            }
        }
        else
        {
            toReturn = Pathfinding.GetAllNodeInDistance(startNode, zoneRange, false);
        }

        return toReturn;
    }
}
