using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSuper : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float maxMovementSpeed;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float airSpeedMultiplier;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected float playerHeight;
    [SerializeField] protected Animator animator;
    [SerializeField] protected CharacterController characterController;
    protected Vector3 velocity;
    protected float gravity = 9.8f;
    protected float rotationSpeed = 7f;
    protected int currentHealth;
    protected bool canJump = true;
    protected float jumpCooldown = 0.25f;
    protected bool canDoubleJump;
    protected bool isGrounded;
    protected bool isPassiv;
    protected bool isStunned;
    protected bool isTough;
    protected bool hasFeatherFall;
    protected bool isKnockingBack;
    protected float knockBackEffectTimer;
    protected float knockbackForce = 7f;
    protected Vector3 knockBackDirection;
    
    

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }

    public float AirSpeedMultiplier
    {
        get => airSpeedMultiplier;
        set => airSpeedMultiplier = value;
    }

    public bool CanDoubleJump
    {
        get => canDoubleJump;
        set => canDoubleJump = value;
    }
    
    public bool IsPassiv
    {
        get => isPassiv;
        set => isPassiv = value;
    }

    public bool IsTough
    {
        get => isTough;
        set => isTough = value;
    }

    public bool HasFeatherFall
    {
        get => hasFeatherFall;
        set => hasFeatherFall = value;
    }

    public bool IsStunned
    {
        get => isStunned;
        set => isStunned = value;
    }

    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }

    public CharacterController CharacterController => characterController;

    protected virtual void VerticalMovement()
    {
        velocity.y -= gravity * Time.deltaTime;
        
        characterController.Move(velocity * Time.deltaTime);
    }
    
    protected IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    protected void CheckMovementSpeed()
    {
        if (movementSpeed > maxMovementSpeed)
        {
            movementSpeed = maxMovementSpeed;
        }
    }

    public void ApplyKnockBack(Vector3 source)
    {
        knockBackDirection = -(source - transform.position).normalized;
        isKnockingBack = true;
        knockBackEffectTimer = 0f;
    }

    protected void KnockBack()
    {
        knockBackEffectTimer += Time.deltaTime;

        if (knockBackEffectTimer < 1.5f)
        {
            float progress = knockBackEffectTimer / 1.5f;
            Vector3 currentKnockback = Vector3.Lerp(knockBackDirection * knockbackForce, Vector3.zero, progress);
            characterController.Move(currentKnockback * Time.deltaTime);
        }
        else
        {
            isKnockingBack = false;
        }
    }

    protected virtual void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
