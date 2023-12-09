using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceImage : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
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
        if (resourceSlot.ResourceObject.Amount > 0)
        {
            resourceSlot.SubFromAmount(1);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.7f;
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetComponentInParent<CraftingSlot>())
        {
            
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            resourceSlot.AddToAmount(1);
            Destroy(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (resourceSlot.ResourceObject.Amount > -1)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            resourceSlot.AddToAmount(1);
            Destroy(gameObject);
        }
    }
}