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
    [SerializeField] private Sprite spriteOfPotion;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject cylinder;
    
    private bool isThrown = true;
    

    public Sprite SpriteOfPotion => spriteOfPotion;

    public Rigidbody Rb => rb;
    
    public void UseEffect(CharacterSuper target)
    {
        if (potionType == PotionType.Pacify)
        {
            StartCoroutine(Pacify(target));
        }
        else if (potionType == PotionType.Healing)
        {
            Heal(target);
        }
        else if (potionType == PotionType.Toughness)
        {
            StartCoroutine(Toughness(target));
        }
        else if (potionType == PotionType.DoubleJump)
        {
            StartCoroutine(DoubleJump(target));
        }
        else if (potionType == PotionType.SpeedBoost)
        {
            StartCoroutine(SpeedBoost(target));
        }
        else if (potionType == PotionType.FeatherFall)
        {
            StartCoroutine(FeatherFall(target));
        }
        else if (potionType == PotionType.Bomb)
        {
            StartCoroutine(Bomb(target));
        }
    }

    public void DrinkEffect(CharacterSuper target)
    {
        if (potionType == PotionType.Pacify)
        {
            StartCoroutine(Pacify(target));}
        else if (potionType == PotionType.Healing)
        {
            Heal(target);
        }
        else if (potionType == PotionType.Toughness)
        {
            StartCoroutine(Toughness(target));
        }
        else if (potionType == PotionType.DoubleJump)
        {
            StartCoroutine(DoubleJump(target));
        }
        else if (potionType == PotionType.SpeedBoost)
        {
            StartCoroutine(SpeedBoost(target));
        }
        else if (potionType == PotionType.FeatherFall)
        {
            StartCoroutine(FeatherFall(target));
        }
        else if (potionType == PotionType.Bomb)
        {
            //Bomb(target);
        }
        Destroy(gameObject);
    }

    private IEnumerator Pacify(CharacterSuper target)
    {
        target.IsPassiv = true;
        yield return new WaitForSeconds(duration);
        target.IsPassiv = false;
    }

    private void Heal(CharacterSuper target)
    {
        if (target.CurrentHealth == target.MaxHealth)return;
        
        target.CurrentHealth += 2;
        
        if (target.CurrentHealth > target.MaxHealth)
        {
            target.CurrentHealth = target.MaxHealth;
        }
    }

    private IEnumerator Toughness(CharacterSuper target)
    {
        target.IsTough = true;
        yield return new WaitForSeconds(duration);
        target.IsTough = false;
    }

    private IEnumerator DoubleJump(CharacterSuper target)
    {
        target.CanDoubleJump = true;
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator SpeedBoost(CharacterSuper target)
    {
        target.MovementSpeed += 2;
        yield return new WaitForSeconds(duration);
        target.MovementSpeed -= 2;
        Destroy(gameObject);
    }

    private IEnumerator FeatherFall(CharacterSuper target)
    {
        target.HasFeatherFall = true;
        target.Gravity = 4f;
        yield return new WaitForSeconds(duration);
        target.HasFeatherFall = false;
        target.Gravity = 9.8f;
    }

    private IEnumerator Bomb(CharacterSuper target)
    {
        target.ApplyKnockBack(transform.position);
        target.CurrentHealth -= 1;
        target.IsStunned = true;
        yield return new WaitForSeconds(duration);
        target.IsStunned = false;
    }

    private IEnumerator DestroyPotion()
    {
        
        yield return new WaitForSeconds(duration+1);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isThrown)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
            foreach (Collider collider in colliders)
            {
                CharacterSuper characterSuper = collider.gameObject.GetComponent<CharacterSuper>();
                if (characterSuper != null)
                {
                    UseEffect(characterSuper);
                }
            }
            sphere.SetActive(false);
            cylinder.SetActive(false);
            isThrown = false;
        }
        
        StartCoroutine(DestroyPotion());
    }
}