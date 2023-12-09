using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform rectTransform;
    
    [CanBeNull] private ResourceImage currentResource;
    
    [CanBeNull] public ResourceImage CurrentResource => currentResource;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject eventObject = eventData.pointerDrag;
        if ( eventObject == null) return;

        if (currentResource != null)
        {
            currentResource.ResourceSlotGetSet.AddToAmount(1);
            Destroy(currentResource.gameObject);
        }
        
        eventObject.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        currentResource = eventObject.GetComponent<ResourceImage>();
        currentResource.transform.SetParent(transform, false);
        currentResource.ResourceSlotGetSet.ResourceImage = null;
    }

    public void ResetSlot()
    {
        if (currentResource != null)
        {
            currentResource.ResourceSlotGetSet.AddToAmount(1);
            Destroy(currentResource.gameObject);
        }
    }

    public void UseResource()
    {
        if (currentResource != null)
        {
            Destroy(currentResource.gameObject);
            currentResource = null;
        }
    }
}
