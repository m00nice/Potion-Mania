using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterSuper
{
    public static Player Instance;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform camera;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float playerHeight;
    [SerializeField] private Animator animator;
    private float rotationSpeed = 7f;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float groundDrag;
    private bool isGrounded;
    private bool canJump = true;
    private float jumpCooldown = 0.25f;

    private float interactRange = 3f;
    private InteractableObjectSuper interactableObjectSuper;


    private void Awake()
    {
        Instance = this;
        movementSpeed = 3f;
        jumpForce = 7f;
        airSpeedMultiplier = 4f;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //load file
        StartCoroutine(GetClosestInteractableObject());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Inventory.Instance.IsActive)
            {
                Inventory.Instance.OpenInventory();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Inventory.Instance.CloseInventory();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        isGrounded = Physics.Raycast(orientation.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isGrounded", false);
            animator.SetBool("isFalling", true);
        }

        CameraRotation();

        Move();

        if (Input.GetAxis("Jump") != 0 && isGrounded && canJump)
        {
            Jump();
            canJump = false;
            StartCoroutine(JumpCooldown());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactableObjectSuper != null)
            {
                interactableObjectSuper.Interact(this);
            }
        }
    }

    private void SetPlayerStats(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void CameraRotation()
    {
        Vector3 viewDirection = transform.position - new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
        orientation.forward = viewDirection.normalized;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (verticalInput == 0 && horizontalInput == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }

            if (isGrounded)
            {
                rb.drag = groundDrag;
                animator.SetBool("isMoving", true);
            }
            else
            {
                rb.drag = 0;
                rb.AddForce(moveDirection.normalized * (movementSpeed * airSpeedMultiplier), ForceMode.Force);
                animator.SetBool("isMoving", false);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        animator.SetBool("isJumping", true);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void Dash()
    {
    }

    private void Melee()
    {
    }

    private void KnockBack()
    {
    }

    private void Duck()
    {
    }
    //

    private void UsePotion()
    {
    }

    private void ThrowPotion()
    {
    }

    private IEnumerator GetClosestInteractableObject()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(1);
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            if (colliders.Length > 0)
            {
                GameObject? closestObject = colliders
                    .Where(collider => collider.gameObject != null && collider.gameObject != gameObject)
                    .Where(collider => collider.GetComponent<InteractableObjectSuper>() != null)
                    .OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position))
                    .Select(collider => collider.gameObject)
                    .FirstOrDefault();
                if (closestObject != null)
                {
                    InteractableObjectSuper? closestInteractableObject = closestObject.GetComponent<InteractableObjectSuper>();

                    if (closestInteractableObject != null)
                    {
                        if (interactableObjectSuper != null)
                        {
                            interactableObjectSuper.HideE();
                        }

                        interactableObjectSuper = closestInteractableObject;
                        interactableObjectSuper.ShowE();
                    }
                    else
                    {
                        if (interactableObjectSuper != null)
                        {
                            interactableObjectSuper.HideE();
                        }

                        interactableObjectSuper = null;
                    }
                }
                else
                {
                    if (interactableObjectSuper != null)
                    {
                        interactableObjectSuper.HideE();
                    }

                    interactableObjectSuper = null;
                }
            }
            else
            {
                if (interactableObjectSuper != null)
                {
                    interactableObjectSuper.HideE();
                }

                interactableObjectSuper = null;
            }
        }
    }
}