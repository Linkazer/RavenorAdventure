using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/Playable Character")]
public class CharacterScriptable_Battle : CharacterScriptable, CPN_Data_HealthHandler, CPN_Data_Movement, CPN_Data_SpellCaster, CPN_Data_EffectHandler
{
    [Header("Display")]
    [SerializeField] private Sprite characterSprite;

    [SerializeField] private float handHeight;
    [SerializeField] private float uiHeight;

    [Header("Combat Stats")]
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private int defense;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;
    [SerializeField] private int relances;

    [Header("Movement")]
    [SerializeField] private int movementByTurn = 35;
    [SerializeField] private float speed = 5;

    [Header("Spells")]
    [SerializeField] private List<SpellScriptable> availableSpells;
    [SerializeField] private int usableSpellByTurn = 1;

    [Header("Passives")]
    [SerializeField] private List<EffectScriptable> passives;

    public Sprite GameSprite()
    {
        return characterSprite;
    }


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

    public float Speed()
    {
        return speed;
    }

    public List<SpellScriptable> AvailableSpells()
    {
        return availableSpells;
    }

    public int MaxSpellUseByTurn()
    {
        return usableSpellByTurn;
    }

    public int Defense()
    {
        return defense;
    }

    public int PossibleRelance()
    {
        return relances;
    }

    public int Accuracy()
    {
        return accuracy;
    }

    public int Power()
    {
        return power;
    }

    public List<EffectScriptable> Effects()
    {
        return new List<EffectScriptable>(passives);
    }
}
