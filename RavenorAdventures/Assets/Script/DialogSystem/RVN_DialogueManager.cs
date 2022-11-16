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

    [SerializeField] private CanvasGroup dialogueGroup;
    [SerializeField] private CanvasGroup responsesGroup;

    [Header("Events")]
    [SerializeField] private UnityEvent onStartDialogue;
    [SerializeField] private UnityEvent onEndDialogue;

    private DialogueScriptable currentDialogue;
    private int currentSentenceIndex;

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
        currentSentenceIndex = 0;

        dialogueGroup.interactable = true;
        responsesGroup.interactable = false;
        responsesGroup.alpha = 0;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        DialogueSentence sentence = currentDialogue.Sentences[currentSentenceIndex];

        if (sentence.talker != null)
        {
            characterName.text = sentence.talker.Nom;
            characterName.color = sentence.talker.NameColor;
        }
        else
        {
            characterName.text = "";
        }

        dialogueText.text = sentence.text.GetText();

        if (sentence.leftCharacter != null)
        {
            leftCharacterDisplay.enabled = true;
            leftCharacterDisplay.sprite = sentence.leftCharacter.Portrait;
        }
        else
        {
            leftCharacterDisplay.enabled = false;
        }
        if (sentence.rightCharacter != null)
        {
            rightCharacterDisplay.enabled = true;
            rightCharacterDisplay.sprite = sentence.rightCharacter.Portrait;
        }
        else
        {
            rightCharacterDisplay.enabled = false;
        }

        currentSentenceIndex++;

        if (currentSentenceIndex >= currentDialogue.Sentences.Length)
        {
            DisplayResponses(currentDialogue);
        }
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

        dialogueGroup.interactable = false;
        responsesGroup.interactable = true;
        responsesGroup.alpha = 1;
    }

    private void EndDialogue()
    {
        onEndDialogue?.Invoke();

        currentDialogue = null;
    }

    public void SelectResponse(int index)
    {
        foreach(DialogueResponseEffect effect in currentDialogue.Responses[index].Effects)
        {
            effect.ApplyEffect();
        }

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
