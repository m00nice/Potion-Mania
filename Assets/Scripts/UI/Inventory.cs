using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private Button exitButton;
    [SerializeField] private ResourceSlot[] resourceSlots;
    [SerializeField] private ResourceObject[] resources;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool isActive = false;

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //load save file
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        isActive = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        SetInventory();
        //Disable movement controls
        //Disable camera controls
    }
    
    public void CloseInventory()
    {
        isActive = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void SetInventory()
    {
        foreach (ResourceSlot resourceSlot in resourceSlots)
        {
            resourceSlot.AmountText.text = resourceSlot.ResourceObject.Amount.ToString();
        }
        
        
    }
}
