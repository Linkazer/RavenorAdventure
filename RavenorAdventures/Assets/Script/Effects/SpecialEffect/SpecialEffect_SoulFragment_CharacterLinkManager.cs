using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect_SoulFragment_CharacterLinkManager : MonoBehaviour
{
    public class LinkCharacterData
    {
        public CPN_Character linkedCharacter;
        public EffectScriptable effect;

        public LinkCharacterData(CPN_Character chara, EffectScriptable eff)
        {
            linkedCharacter = chara;
            effect = eff;
        }
    }
    
    public static SpecialEffect_SoulFragment_CharacterLinkManager Instance;

    private List<LinkCharacterData> linkedCharacters = new List<LinkCharacterData>();

    private void Awake()
    {
        Instance = this;
    }

    public bool IsCharacterLinked(CPN_Character character, EffectScriptable effect)
    {
        foreach(LinkCharacterData linkedChara in linkedCharacters)
        {
            if(linkedChara.linkedCharacter == character && linkedChara.effect == effect)
            {
                return true;
            }
        }

        return false;
    }

    public void AddCharacter(CPN_Character character, EffectScriptable effect)
    {
        linkedCharacters.Add(new LinkCharacterData(character, effect));
    }

    public void RemoveCharacter(CPN_Character character, EffectScriptable effect)
    {
        for(int i = 0; i < linkedCharacters.Count; i++)
        {
            if (linkedCharacters[i].linkedCharacter == character && linkedCharacters[i].effect == effect)
            {
                linkedCharacters.RemoveAt(i);
                break;
            }
        }
    }
}
