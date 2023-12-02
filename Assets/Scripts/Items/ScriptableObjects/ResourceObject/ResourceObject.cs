using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceObject", menuName = "New ResourceObject")]
public class ResourceObject : ScriptableObject
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    protected int amount;

    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    public GameObject Prefab => prefab;
    
    public Sprite Sprite => sprite;
    
    
    
}
