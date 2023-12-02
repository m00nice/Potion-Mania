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
    private float duration;
    private PotionType potionType;
    

    public void UseEffect(GameObject target)
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

    private IEnumerator SpeedBoost(GameObject target)
    {
        CharacterSuper targetCS = target.GetComponent<CharacterSuper>();
        targetCS.MovementSpeed += 5;
        yield return new WaitForSeconds(10);
        targetCS.MovementSpeed -= 5;
    }
}
