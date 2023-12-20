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
    [SerializeField] private Image previewSlot;
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
            if (recipeObject.TopSlot?.Equals(topSlot.CurrentResource?.ResourceSlotGetSet.ResourceObject) == true &&
                recipeObject.MiddleSlot?.Equals(middleSlot.CurrentResource?.ResourceSlotGetSet.ResourceObject) == true &&
                recipeObject.RightSlot?.Equals(rightSlot.CurrentResource?.ResourceSlotGetSet.ResourceObject) == true &&
                recipeObject.LeftSlot?.Equals(leftSlot.CurrentResource?.ResourceSlotGetSet.ResourceObject) == true &&
                recipeObject.BottomSlot?.Equals(bottomSlot.CurrentResource?.ResourceSlotGetSet.ResourceObject) == true)
            {
                //Set preview to recipeObject.Potion.Sprite
                previewSlot.sprite = recipeObject.Potion.PotionPrefab.SpriteOfPotion;
                craftButton.onClick.AddListener(() =>
                {
                    recipeObject.Potion.PotionAmount++;
                    if (topSlot.CurrentResource != null)
                    {
                        Destroy(topSlot.CurrentResource.gameObject);
                    }
                    if (middleSlot.CurrentResource != null)
                    {
                        Destroy(middleSlot.CurrentResource.gameObject);
                    }
                    if (rightSlot.CurrentResource != null)
                    {
                        Destroy(rightSlot.CurrentResource.gameObject);
                    }
                    if (leftSlot.CurrentResource != null)
                    {
                        Destroy(leftSlot.CurrentResource.gameObject);
                    }
                    if (bottomSlot.CurrentResource != null)
                    {
                        Destroy(bottomSlot.CurrentResource.gameObject);
                    }
                    
                    
                });
            }
        }
    }
}
