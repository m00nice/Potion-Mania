using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeObject", menuName = "New RecipeObject")]
public class RecipeObject : ScriptableObject
{
    [SerializeField] [CanBeNull] private ResourceObject topSlot;
    [SerializeField] [CanBeNull] private ResourceObject middleSlot;
    [SerializeField] [CanBeNull] private ResourceObject rightSlot;
    [SerializeField] [CanBeNull] private ResourceObject leftSlot;
    [SerializeField] [CanBeNull] private ResourceObject bottomSlot;
    [SerializeField] private Potion product;

    public ResourceObject TopSlot => topSlot;

    public ResourceObject MiddleSlot => middleSlot;

    public ResourceObject RightSlot => rightSlot;

    public ResourceObject LeftSlot => leftSlot;

    public ResourceObject BottomSlot => bottomSlot;

    public Potion Product => product;
}
