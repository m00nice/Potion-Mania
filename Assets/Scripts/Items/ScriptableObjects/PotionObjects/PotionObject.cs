using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionObject", menuName = "New PotionObject")]
public class PotionObject : ScriptableObject
{
    [SerializeField] private int potionAmount;
    [SerializeField] private Potion potionPrefab;
    
    public int PotionAmount
    {
        get => potionAmount;
        set => potionAmount = value;
    }

    public Potion PotionPrefab => potionPrefab;
}
