using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverShowImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] public bool IsAvailable = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAvailable)
        {
            SoundManager.instance.Play("ButtonHover");

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
            }

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsAvailable)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0;
            }

        }
    }

    public void ImplicitHide()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0;
        }

    }
}
