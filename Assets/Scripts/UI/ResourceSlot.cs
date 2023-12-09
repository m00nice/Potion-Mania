using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ResourceObject resourceObject;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private ResourceImage resourceImagePrefab;
    [SerializeField] private ResourceImage resourceImage;

    public ResourceObject ResourceObject
    {
        get => resourceObject;
    }
    
    public ResourceImage ResourceImage
    {
        get => resourceImage;
        set => resourceImage = value;
    }

    private void Start()
    {
        StartCoroutine(SpawnResourceImage());
    }

    private IEnumerator SpawnResourceImage()
    {
        while (Application.isPlaying)
        {
            if (resourceImage == null)
            {
                resourceImage = Instantiate(resourceImagePrefab, transform);
                resourceImage.Canvas = canvas;
                resourceImage.ResourceSlotGetSet = this;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void AddToAmount(int value)
    {
        ResourceObject.Amount += value;
        amountText.text = ResourceObject.Amount.ToString();
    }
    
    public void SubFromAmount(int value)
    {
        ResourceObject.Amount -= value;
        amountText.text = ResourceObject.Amount.ToString();
    }
}