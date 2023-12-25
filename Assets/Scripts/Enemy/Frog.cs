using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Frog : CharacterSuper
{
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform tongue;
    [SerializeField] private Vector3 tongueScale;
    [SerializeField] private Vector3 tonguePos;
    [SerializeField] private Vector3 tongueScaleAttack;
    [SerializeField] private Vector3 tonguePosAttack;
    private bool playerInAggroRange;
    private bool playerInAttackRange;
    private Vector3 destination;
    private bool hasDestination;
    private bool shouldJump;
    private bool isAttacking;
    private bool hasAttacked;

    private void Start()
    {
        StartCoroutine(FrogJump());
        StartCoroutine(FrogBored());
        currentHealth = maxHealth;
    }

    void Update()
    {
        Die();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        KnockBack();
        VerticalMovement();
        if(isStunned)return;
        if (isPassiv) return;
        playerInAggroRange = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInAggroRange && !playerInAttackRange) PatrolBehaviour();
        if (playerInAggroRange && !playerInAttackRange) ChaseBehaviour();
        if (playerInAggroRange && playerInAttackRange) AttackBehaviour();
        
        
    }

    protected override void VerticalMovement()
    {
        if (shouldJump && isGrounded)
        {
            velocity.y = jumpForce;
            shouldJump = false;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
    

    private void PatrolBehaviour()
    {
        if (!hasDestination) GetNewDestination();
        FaceTarget(destination);
        characterController.Move(transform.forward * Time.deltaTime);

        Vector3 distanceToDestination = transform.position - destination;

        if (distanceToDestination.magnitude < 1f)
        {
            hasDestination = false;
        }
    }

    private void ChaseBehaviour()
    {
        FaceTarget(player.position);
        characterController.Move(transform.forward * Time.deltaTime);
    }

    private void AttackBehaviour()
    {
        if(isAttacking)return;
        FaceTarget(player.position);
        characterController.Move(transform.forward * Time.deltaTime);
        StartCoroutine(Attack(player.position));
    }

    private void GetNewDestination()
    {
        float randomZ = Random.Range(-aggroRange, aggroRange);
        float randomX = Random.Range(-aggroRange, aggroRange);

        destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(destination, Vector3.down, 2, ground))
        {
            hasDestination = true;
        }
        else
        {
            hasDestination = false;
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }

    private IEnumerator Attack(Vector3 target)
    {
        if (!isAttacking && !hasAttacked)
        {
            tongue.localPosition = tonguePosAttack;
            tongue.localScale = tongueScaleAttack;
            isAttacking = true;
            hasAttacked = true;
            yield return new WaitForSeconds(1.0f);
            tongue.localPosition = tonguePos;
            tongue.localScale = tongueScale;
            isAttacking = false;
            yield return new WaitForSeconds(0.5f);
            hasAttacked = false;
        }
    }

    private IEnumerator FrogJump()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(5.0f);
            shouldJump = true;
        }
    }

    private IEnumerator FrogBored()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(45.0f);
            if (!playerInAggroRange)
            {
                GetNewDestination();
            }
        }
    }
}