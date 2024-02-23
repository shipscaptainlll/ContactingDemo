using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _conversationCanvas;
    [SerializeField] private LookAtConversation _lookAtConversation;
    [SerializeField] private DialogueManager _dialogueManager;

    [SerializeField] private HoverShowImage _showImageFirst;
    [SerializeField] private HoverShowImage _showImageSecond;
    [SerializeField] private HoverShowImage _showImageThird;
    private InputHandler _input;
    public bool IsConversing;
    public LookAtPlayer _currentLookAtPlayer;
    public Transform CurrentLookAtCharacter { get; private set; }

    public event Action ConversationStarted = delegate { };
    public event Action ConversationFinished = delegate { };

    private void Start()
    {
        _input = new InputHandler();
        _input.Player.Enable();

        //_input.Player.Escape.started += (callback) => { FinishConversation(); };
    }


    public void InitiateConversation(Dialogue dialogue, LookAtPlayer lookAtPlayer, Transform lookAtCharacter)
    {
        if (IsConversing)
        {
            return;
        }

        CurrentLookAtCharacter = lookAtCharacter;
        _currentLookAtPlayer = lookAtPlayer;
        _currentLookAtPlayer.RotateTowardsTarget();
        ConversationStarted?.Invoke();
        IsConversing = true;
        _dialogueManager.SetCurrentDialogue(dialogue);
        _conversationCanvas.alpha = 1;
        _conversationCanvas.blocksRaycasts = true;
        _conversationCanvas.interactable = true;
        _lookAtConversation.ZoomIn();

    }

    public void FinishConversation()
    {
        if (!IsConversing)
        {
            return;
        }

        _currentLookAtPlayer.StopRotation();
        ConversationFinished?.Invoke();
        IsConversing = false;
        _conversationCanvas.alpha = 0;
        _conversationCanvas.blocksRaycasts = false;
        _conversationCanvas.interactable = false;
        _showImageFirst.ImplicitHide();
        _showImageSecond.ImplicitHide();
        _showImageThird.ImplicitHide();

        _lookAtConversation.ZoomOut();
    }
}
