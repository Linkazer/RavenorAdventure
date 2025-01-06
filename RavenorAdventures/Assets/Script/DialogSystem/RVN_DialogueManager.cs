using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RVN_DialogueManager : RVN_Singleton<RVN_DialogueManager>
{
    [SerializeField] private AudioData dialogueMusic;
    [SerializeField] private Color speakColor = Color.black;
    [SerializeField] private Color descriptionColor = new Color(0.37f, 0.16f, 0.05f);
    [SerializeField] private Color linkedSpeakColor = Color.magenta;
    [SerializeField] private Color linkedDescriptionColor = Color.magenta;

    [Header("Dialogue setup")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup characterGroup;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image leftCharacterDisplay;
    //[SerializeField] private Image rightCharacterDisplay;
    [SerializeField] private List<DialogueResponseHandler> responses;

    [SerializeField] private Button dialogueButton;
    [SerializeField] private CanvasGroup dialogueGroup;
    [SerializeField] private CanvasGroup responsesGroup;

    [Header("Events")]
    [SerializeField] private UnityEvent onStartDialogue;
    [SerializeField] private UnityEvent onEndDialogue;

    private AudioData musicOutsideDialogue;
    private DialogueScriptable currentDialogue;
    private int currentSentenceIndex;

    private Action onDialogueEndCallback;

    public static void PlayDialogue(DialogueScriptable toPlay)
    {
        instance.DisplayDialogue(toPlay);
    }

    public static void PlayDialogue(DialogueScriptable toPlay, Action callback)
    {
        instance.onDialogueEndCallback = callback;

        PlayDialogue(toPlay);
    }

    private void DisplayDialogue(DialogueScriptable toPlay)
    {
        if(currentDialogue == null)
        {
            onStartDialogue?.Invoke();

            MusicManager.instance.AskToPlayMusic(dialogueMusic, false);
        }

        currentDialogue = toPlay;
        currentSentenceIndex = 0;

        dialogueGroup.interactable = true;
        responsesGroup.interactable = false;
        responsesGroup.alpha = 0;

        dialogueButton.interactable = true;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentSentenceIndex >= currentDialogue.Sentences.Length)
        {
            EndDialogue();
            return;
        }

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

        string[] systemSentences = sentence.text.GetText().Split('<', '>');

        string fullSentence = "";
        bool isSystem = true;
        int systemCount = 0;

        bool isSpeaking = false;

        foreach (string systStr in systemSentences)
        {
            if(fullSentence != "" && isSystem)
            {
                fullSentence += $">";

                if(systemCount % 2 == 0)
                {
                    fullSentence += "</color>";
                }
            }

            isSystem = !isSystem;

            if(isSystem)
            {
                if (systemCount % 2 == 0)
                {
                    if (isSpeaking)
                    {
                        fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(linkedSpeakColor)}>";
                    }
                    else
                    {
                        fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(linkedDescriptionColor)}>";
                    }
                }

                systemCount++;

                fullSentence += "<" + systStr;
            }
            else
            {
                string[] formedSentence = systStr.Split('“', '”', '"');

                for(int i = 0; i < formedSentence.Length; i++)
                {
                    if (i != 0)
                    {
                        isSpeaking = !isSpeaking;

                        if (!isSpeaking)
                        {
                            fullSentence += "”";
                            fullSentence += "</color>";
                        }

                        if (isSpeaking)
                        {
                            fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(speakColor)}>“";
                        }
                        else
                        {
                            fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(descriptionColor)}>";
                        }
                    }

                    fullSentence += formedSentence[i];
                }
            }
        }

        fullSentence += "</color>";

        dialogueText.text = fullSentence;

        if (sentence.talker != null)
        {
            characterGroup.alpha = 1;
            characterGroup.blocksRaycasts = true;

            if (sentence.talker.Portrait != null)
            {
                leftCharacterDisplay.color = Color.white;
                leftCharacterDisplay.sprite = sentence.talker.Portrait;
            }
            else
            {
                leftCharacterDisplay.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            characterGroup.alpha = 0;
            characterGroup.blocksRaycasts = false;
        }

        /*if (sentence.rightCharacter != null)
        {
            rightCharacterDisplay.enabled = true;
            rightCharacterDisplay.sprite = sentence.rightCharacter.Portrait;
        }
        else
        {
            rightCharacterDisplay.enabled = false;
        }*/

        currentSentenceIndex++;

        /*if (currentSentenceIndex >= currentDialogue.Sentences.Length)
        {
            DisplayResponses(currentDialogue);
        }*/
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
        onDialogueEndCallback?.Invoke();
        onEndDialogue?.Invoke();

        MusicManager.instance.PlayMainMusic();

        dialogueButton.interactable = false;
        currentDialogue = null;
        onDialogueEndCallback = null;
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
