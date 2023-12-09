using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{
    Pacify,
    Healing,
    Toughness,
    DoubleJump,
    SpeedBoost,
    FeatherFall,
    Bomb,
    
}
public class Potion : MonoBehaviour
{
    [SerializeField] private PotionType potionType;
    [SerializeField] private float duration;
    private bool isThrown = true;
    

    public void UseEffect(CharacterSuper target)
    {
        if (potionType == PotionType.Pacify)
        {
            
        }
        else if (potionType == PotionType.Healing)
        {
            
        }
        else if (potionType == PotionType.Toughness)
        {
            
        }
        else if (potionType == PotionType.DoubleJump)
        {
            
        }
        else if (potionType == PotionType.SpeedBoost)
        {
             SpeedBoost(target);
        }
        else if (potionType == PotionType.FeatherFall)
        {
            
        }
        else if (potionType == PotionType.Bomb)
        {
            
        }
        
    }

    private void Heal(GameObject target)
    {
        
    }

    private IEnumerator SpeedBoost(CharacterSuper target)
    {
        target.MovementSpeed += 5;
        yield return new WaitForSeconds(duration);
        target.MovementSpeed -= 5;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isThrown)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
            Debug.Log("Potion Crash");
            foreach (Collider collider  in colliders)
            {
                CharacterSuper characterSuper = collider.gameObject.GetComponent<CharacterSuper>();
                if (characterSuper != null)
                {
                    UseEffect(characterSuper);
                }
            }
        }
    }
}
