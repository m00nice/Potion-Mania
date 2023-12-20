using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSuper : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float airSpeedMultiplier;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected float playerHeight;
    [SerializeField] protected Animator animator;
    [SerializeField] protected CharacterController characterController;
    protected float gravity = 9.8f;
    protected float fallSpeed;
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

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            fallSpeed += gravity * Time.deltaTime;
        }
        else
        {
            fallSpeed = 0f;
        }

        characterController.Move(Vector3.down * (fallSpeed * Time.deltaTime));
    }
}
