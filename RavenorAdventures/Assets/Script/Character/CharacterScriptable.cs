using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/Playable Character")]
public class CharacterScriptable : ScriptableObject, CPN_Data_HealthHandler, CPN_Data_Movement, CPN_Data_SpellCaster
{
    [Header("Display")]
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private Sprite UIPortrait;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxArmor;

    [Header("Movement")]
    [SerializeField] private int movementByTurn;
    [SerializeField] private float speed;

    [Header("Spells")]
    [SerializeField] private List<SpellScriptable> availableSpells;
    [SerializeField] private int usableSpellByTurn;

    public Sprite GameSprite()
    {
        return characterSprite;
    }

    public Sprite UISprite()
    {
        return UIPortrait;
    }

    public float MaxArmor()
    {
        return maxArmor;
    }

    public float MaxHealth()
    {
        return maxHealth;
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

}
