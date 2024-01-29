using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterChoice : MonoBehaviour
{
    [SerializeField] private List<UI_CharacterSelector> characterSelectors;

    [Header("Grid Layout")]
    [SerializeField] private GridLayoutGroup layoutGroup;
    [SerializeField] private float spaceOnGrouped;
    [SerializeField] private float spaceOnNotGrouped;

    [Header("Group Button")]
    [SerializeField] private GameObject groupButton;

    private void Start()
    {
        RVN_FreeRoamingManager.Instance.actOnEnableRoaming += OnEnableRoaming;
    }

    private void OnDestroy()
    {
        if(RVN_FreeRoamingManager.Instance != null)
        {
            RVN_FreeRoamingManager.Instance.actOnEnableRoaming -= OnEnableRoaming;
        }
    }

    private void OnEnableRoaming(bool isRoamingEnable)
    {
        groupButton.SetActive(isRoamingEnable);

        UpdateLayoutGrid();
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

    public void UE_ChangeGroupMode()
    {
        RVN_FreeRoamingManager.Instance.SetGrouped(!RVN_FreeRoamingManager.Instance.AreCharacterGrouped);

        UpdateLayoutGrid();
    }

    private void UpdateLayoutGrid()
    {
        if (RVN_FreeRoamingManager.Instance.AreCharacterGrouped)
        {
            layoutGroup.spacing = new Vector2(layoutGroup.spacing.x, spaceOnGrouped);
        }
        else
        {
            layoutGroup.spacing = new Vector2(layoutGroup.spacing.x, spaceOnNotGrouped);
        }
    }
}
