using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform rectTransform;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROP DA MIC");
        GameObject eventObject = eventData.pointerDrag;
        if ( eventObject != null)
        {
            eventObject.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        }
    }
}
