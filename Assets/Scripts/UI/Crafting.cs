using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField] private CraftingSlot topSlot;
    [SerializeField] private CraftingSlot middleSlot;
    [SerializeField] private CraftingSlot rightSlot;
    [SerializeField] private CraftingSlot leftSlot;
    [SerializeField] private CraftingSlot bottomSlot;
    [SerializeField] private GameObject previewSlot;
    [SerializeField] private Button craftButton;
    [SerializeField] private RecipeObject[] recipeObjects;

    private void Start()
    {
        StartCoroutine(CheckCraftingSlots());
    }

    private IEnumerator CheckCraftingSlots()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (RecipeObject recipeObject in recipeObjects)
        {
            if (recipeObject.TopSlot == topSlot.CurrentResource.ResourceSlotGetSet.ResourceObject &&
                recipeObject.MiddleSlot == middleSlot.CurrentResource.ResourceSlotGetSet.ResourceObject &&
                recipeObject.RightSlot == rightSlot.CurrentResource.ResourceSlotGetSet.ResourceObject &&
                recipeObject.LeftSlot == leftSlot.CurrentResource.ResourceSlotGetSet.ResourceObject &&
                recipeObject.BottomSlot == bottomSlot.CurrentResource.ResourceSlotGetSet.ResourceObject)
            {
                //Set preview to recipeObject.Potion.Sprite
                //Button function to add that potion
            }
        }
    }
}
