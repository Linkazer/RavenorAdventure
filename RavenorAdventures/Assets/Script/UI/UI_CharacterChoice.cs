using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterChoice : MonoBehaviour
{
    [SerializeField] private List<UI_CharacterSelector> characterSelectors;

    private void Start()
    {
        RVN_BattleManager.Instance.OnSpawnAlly += AddCharacter;
    }

    private void OnDestroy()
    {
        if(RVN_BattleManager.Instance != null)
        {
            RVN_BattleManager.Instance.OnSpawnAlly -= AddCharacter;
        }
    }

    public void SelectCharacter(int index)
    {
        RVN_CombatInputController.ChangeCharacter(index);
    }

    public void SetCharacterLength(List<CPN_Character> playerTeam)
    {
        for(int i = 0; i < characterSelectors.Count; i++)
        {
            if(i < playerTeam.Count)
            {
                characterSelectors[i].Set(playerTeam[i]);
            }
            else
            {
                characterSelectors[i].Unset();
            }
        }
    }

    public void AddCharacter(CPN_Character toAdd)
    {
        for (int i = 0; i < characterSelectors.Count; i++)
        {
            if(!characterSelectors[i].gameObject.activeSelf)
            {
                characterSelectors[i].Set(toAdd);

                break;
            }
        }
    }
}
