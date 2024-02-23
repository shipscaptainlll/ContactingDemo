using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorVisibility : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ConversationManager _conversationManager;

    [Header("Settings")]
    [SerializeField] private bool _hideOnStart;
    [SerializeField] private bool _modeNone;

    private void Start()
    {

        if (_conversationManager != null)
        {
            _conversationManager.ConversationStarted += ShowCursorBorderless;
            _conversationManager.ConversationFinished += HideCursor;
        }

        if (_modeNone)
        {
            ShowCursorBorderless();
            return;
        }

        if (_hideOnStart)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursorBorderless()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
