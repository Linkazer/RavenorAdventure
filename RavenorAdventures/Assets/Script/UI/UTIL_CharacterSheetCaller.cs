using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTIL_CharacterSheetCaller : MonoBehaviour
{
    public void DisplayCharacterSheet(CPN_ClicHandler toDisplay)
    {
        UI_CharacterSheet.TrySetCharacter(toDisplay);
    }
}
