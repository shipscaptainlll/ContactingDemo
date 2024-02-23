using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactableCursorVisibility : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ConversationManager _conversationManager;

    [Header("Setups")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _fullVisibilityCanvasGroup;
    [SerializeField] private PlayerRaycast _playerRaycast;

    private void Awake()
    {

        if (_conversationManager != null)
        {
            _conversationManager.ConversationStarted += Hide;
            _conversationManager.ConversationFinished += Show;
        }


        _playerRaycast.FoundRaycastable += ShowRaycastableInfo;
        _playerRaycast.LostRaycastable += HideRaycastableInfo;
    }

    private void ShowRaycastableInfo(RaycastHit hit)
    {
        hit.transform.TryGetComponent<RaycastableObject>(out RaycastableObject raycastable);

        if (raycastable != null)
        {
            _canvasGroup.alpha = 1;
        }
    }

    public void HideRaycastableInfo()
    {
        _canvasGroup.alpha = 0;
    }

    private void Show()
    {
        _fullVisibilityCanvasGroup.alpha = 1;
    }

    private void Hide()
    {
        _fullVisibilityCanvasGroup.alpha = 0;
    }

}
