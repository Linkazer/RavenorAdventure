using ravenor.referencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/Playable Character")]
public class CharacterScriptable_Battle : CharacterScriptable, CPN_Data_HealthHandler, CPN_Data_Movement, CPN_Data_SpellCaster, CPN_Data_EffectHandler
{
    [Header("Display")]
    [SerializeField] private Sprite characterSprite;

    public SpriteAnimation[] animations;

    [SerializeField] private Sprite handSprite;
    [SerializeField] private bool displayHand;
    [SerializeField] private float handHeight;
    [SerializeField] private float uiHeight;

    [Header("Combat Stats")]
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private int defense;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;
    [SerializeField] private int offensiveRerolls;
    [SerializeField] private int defensiveRerolls;

    [Header("Movement")]
    [SerializeField] private int movementByTurn = 35;
    [SerializeField] private float speed = 5;
    [SerializeField, Tooltip("Score de dangerosité pour 10 de distance (soit une case en ligne droite)")] private float dangerosityMaxDistance;
    [SerializeField, Tooltip("Distance à partir de laquelle le score de dangerosité diminue")] private int dangerosityMinimumDistance;

    [Header("Spells")]
    [SerializeField] private SPL_SpellScriptable opportunitySpell;
    [SerializeField] private List<SPL_SpellScriptable> availableSpells;
    [SerializeField] private int usableSpellByTurn = 1;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SpellRessource))] private SpellRessource usedRessource;

    [Header("Passives")]
    [SerializeField] private List<EffectScriptable> passives;

    /// <summary>
    /// The sprite used InGame.
    /// </summary>
    /// <returns>The sprite used InGame.</returns>
    public Sprite GameSprite()
    {
        return characterSprite;
    }

    public Sprite HandSprite => handSprite;

    public bool DisplayHand => displayHand;

    public float HandHeight => handHeight;

    /// <summary>
    /// La position de l'UI par rapport au pieds du personnage.
    /// </summary>
    public float UIHeight => uiHeight;

    public int MaxArmor()
    {
        return armor;
    }

    public int MaxHealth()
    {
        return health;
    }

    public int MaxDistance()
    {
        return movementByTurn;
    }

    public int DangerosityMinimumDistance => dangerosityMinimumDistance;

    /// <summary>
    /// Différence entre la Min distance et la Max distance de la dangerosité
    /// </summary>
    public float DangerosityLerpDistance => dangerosityMaxDistance - dangerosityMinimumDistance;// + 5f; //Le +5 est là pour faire en sorte que la dernière case de la distance max soit prise en compte

    public float Speed()
    {
        return speed;
    }

    public List<SPL_SpellScriptable> AvailableSpells()
    {
        return availableSpells;
    }

    public SPL_SpellScriptable OpportunitySpell()
    {
        return opportunitySpell;
    }

    public int MaxSpellUseByTurn()
    {
        return usableSpellByTurn;
    }

    public int Defense()
    {
        return defense;
    }

    public int OffensiveRerolls()
    {
        return offensiveRerolls;
    }

    public int DefensiveRerolls()
    {
        return defensiveRerolls;
    }

    public int Accuracy()
    {
        return accuracy;
    }

    public int Power()
    {
        return power;
    }

    /// <summary>
    /// Passifs du personnage.
    /// </summary>
    /// <returns>Passifs du personnage.</returns>
    public List<EffectScriptable> Effects()
    {
        return new List<EffectScriptable>(passives);
    }

    public SpellRessource Ressource()
    {
        return usedRessource;
    }
}
