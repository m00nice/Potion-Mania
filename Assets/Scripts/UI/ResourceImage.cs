using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceImage : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private ResourceSlot resourceSlot;
    private Canvas canvas;

    public ResourceSlot ResourceSlotGetSet
    {
        get => resourceSlot;
        set => resourceSlot = value;
    }

    public Canvas Canvas
    {
        get => canvas;
        set => canvas = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (resourceSlot.ResourceObject.Amount != 0)
        {
            resourceSlot.ResourceObject.Amount--;
            resourceSlot.AmountText.text = resourceSlot.ResourceObject.Amount.ToString();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.7f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (resourceSlot.ResourceObject.Amount > -1)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}