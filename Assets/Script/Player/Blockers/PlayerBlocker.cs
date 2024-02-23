using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocker : MonoBehaviour
{
    [Header("Optional dependencies")]
    [SerializeField] private ConversationManager _conversationManager;
    private bool _conversationInitiated;

    public bool MovementBlocked { get; private set; }
    public bool RaycastingBlocked { get; private set; }
    public bool ContactingBlocked { get; private set; }

    void Awake()
    {
        if (_conversationManager != null)
        {
            _conversationManager.ConversationStarted += () => { _conversationInitiated = true; UpdateBlockages(); };
            _conversationManager.ConversationFinished += () => { _conversationInitiated = false; UpdateBlockages(); };
        }
    }

    private void UpdateBlockages()
    {
        UpdateMovementBlockage();
        UpdateRaycastingBlockage();
        UpdateContactingBlockage();
    }

    private void UpdateMovementBlockage()
    {
        MovementBlocked = (_conversationInitiated);
    }

    private void UpdateRaycastingBlockage()
    {
        RaycastingBlocked = (_conversationInitiated);
    }

    private void UpdateContactingBlockage()
    {
        ContactingBlocked = (_conversationInitiated);
    }
}
