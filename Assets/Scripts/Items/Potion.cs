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
    private bool isThrown = true;

    public Sprite SpriteOfPotion => spriteOfPotion;

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
            //Bomb(target);
        }
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
        target.CanDoubleJump = false;
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
        yield return new WaitForSeconds(duration);
        target.HasFeatherFall = false;
    }

    private IEnumerator Bomb(CharacterSuper target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
        }
        else
        {
            Vector3 awayFromBomb = (target.transform.position - transform.position).normalized;
            targetRb.AddForce(awayFromBomb * 10.0f, ForceMode.Impulse);
            target.CurrentHealth -= 1;
        }

        target.IsStunned = true;
        yield return new WaitForSeconds(duration);
        target.IsStunned = false;
    }

    private IEnumerator DestroyPotion()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isThrown)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
            Debug.Log("Potion Crash");
            foreach (Collider collider in colliders)
            {
                CharacterSuper characterSuper = collider.gameObject.GetComponent<CharacterSuper>();
                if (characterSuper != null)
                {
                    Debug.Log(characterSuper);
                    UseEffect(characterSuper);
                }
            }

            isThrown = false;
        }

        StartCoroutine(DestroyPotion());
    }
}