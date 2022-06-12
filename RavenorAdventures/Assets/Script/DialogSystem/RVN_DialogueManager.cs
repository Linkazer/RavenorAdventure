using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RVN_DialogueManager : RVN_Singleton<RVN_DialogueManager>
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image leftCharacterDisplay;
    [SerializeField] private Image rightCharacterDisplay;
    [SerializeField] private List<DialogueResponseHandler> responses;

    [Header("Events")]
    [SerializeField] private UnityEvent onStartDialogue;
    [SerializeField] private UnityEvent onEndDialogue;

    private DialogueScriptable currentDialogue;

    protected override void OnAwake()
    {
        gameObject.SetActive(false);
    }

    public static void PlayDialogue(DialogueScriptable toPlay)
    {
        instance.DisplayDialogue(toPlay);
    }

    private void DisplayDialogue(DialogueScriptable toPlay)
    {
        if(currentDialogue == null)
        {
            onStartDialogue?.Invoke();
        }

        currentDialogue = toPlay;

        characterName.text = toPlay.Talker.Nom;
        characterName.color = toPlay.Talker.NameColor;

        dialogueText.text = toPlay.Text.GetText();

        leftCharacterDisplay.sprite = toPlay.LeftCharacter.Portrait;
        rightCharacterDisplay.sprite = toPlay.RightCharacter.Portrait;

        DisplayResponses(toPlay);
    }

    private void DisplayResponses(DialogueScriptable toPlay)
    {
        for(int i = 0; i < responses.Count; i++)
        {
            if(i < toPlay.Responses.Count)
            {
                responses[i].SetResponse(toPlay.Responses[i]);
            }
            else
            {
                responses[i].UnsetResponse();
            }
        }
    }

    private void EndDialogue()
    {
        onEndDialogue?.Invoke();

        currentDialogue = null;
    }

    public void SelectResponse(int index)
    {
        if(currentDialogue.Responses[index].NextDialogue != null)
        {
            DisplayDialogue(currentDialogue.Responses[index].NextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }
}
