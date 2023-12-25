using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private ResourceImage noneRes;
    private ResourceImage currentResource;
    public ResourceImage CurrentResource => currentResource;

    private void Start()
    {
        StartCoroutine(SetToNone());
    }

    private IEnumerator SetToNone()
    {
        while (Application.isPlaying)
        {
            if (currentResource == null)
            {
                currentResource = Instantiate(noneRes);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject eventObject = eventData.pointerDrag;
        if (eventObject == null) return;

        if (currentResource != null)
        {
            if (currentResource.ResourceSlotGetSet != null)
            {
                currentResource.ResourceSlotGetSet.AddToAmount(1);
            }
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
            if (currentResource.ResourceSlotGetSet != null)
            {
                currentResource.ResourceSlotGetSet.AddToAmount(1);
            }
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