using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSuper : MonoBehaviour
{
    protected int maxHealth;
    protected int currentHealth;
    protected float movementSpeed;
    protected float jumpForce;
    protected float airSpeedMultiplier;
    protected bool canDoubleJump;

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
}
