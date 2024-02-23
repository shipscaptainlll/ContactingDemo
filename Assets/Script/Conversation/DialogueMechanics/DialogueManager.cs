using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Dialogue _currentDialogue;
    [SerializeField] private DialogueUpdater _dialogueUpdater;

    public event Action<int> DialogueOptionChosen = delegate { };

    public void SetCurrentDialogue(Dialogue newDialogue)
    {
        _currentDialogue = newDialogue;
        _dialogueUpdater.UpdateNextDialogue(_currentDialogue);
    }

    public void PushFirstOption()
    {
        _currentDialogue = _currentDialogue.Variant1Dialogue;
        _dialogueUpdater.UpdateNextDialogue(_currentDialogue);
        DialogueOptionChosen?.Invoke(1);
    }

    public void PushSecondOption()
    {
        _currentDialogue = _currentDialogue.Variant2Dialogue;
        _dialogueUpdater.UpdateNextDialogue(_currentDialogue);
        DialogueOptionChosen?.Invoke(2);
    }

    public void PushThirdOption()
    {
        _currentDialogue = _currentDialogue.Variant3Dialogue;
        _dialogueUpdater.UpdateNextDialogue(_currentDialogue);
        DialogueOptionChosen?.Invoke(3);
    }
}
