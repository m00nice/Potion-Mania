using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipyObject : ScriptableObject
{
    [SerializeField] private ResourceObject topSlot;
    [SerializeField] private ResourceObject middleSlot;
    [SerializeField] private ResourceObject rightSlot;
    [SerializeField] private ResourceObject leftSlot;
    [SerializeField] private ResourceObject bottomSlot;
    [SerializeField] private Potion product;
}
