using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Player : CharacterSuper
{
    public static Player Instance;
    [SerializeField] private Transform orientation;
    [SerializeField] private PotionObject[] potionObjects;
    [SerializeField] private Transform potionThrowSpawn;
    private PotionObject selectedPotion;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float interactRange = 3f;
    private InteractableObjectSuper interactableObjectSuper;
    private bool isCrafting;
    private bool inventoryActive;
    private bool isDrinking;

    public PotionObject SelectedPotion => selectedPotion;
    public bool InventoryActive => inventoryActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SelectPotion(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        StartCoroutine(GetClosestInteractableObject());
    }
    
    private void Update()
    {
        Die();
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!Inventory.Instance.IsActive)
            {
                Inventory.Instance.OpenInventory();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                inventoryActive = true;
            }
            else
            {
                Inventory.Instance.CloseInventory();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                inventoryActive = false;
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
        if (inventoryActive) return;
        VerticalMovement();
        KnockBack();
        if (isStunned) return;

        CheckMovementSpeed();
        Move();
        
        if (!canJump)
        {
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactableObjectSuper != null)
            {
                interactableObjectSuper.Interact(this);
            }
        }

        if (Input.mouseScrollDelta.y > 0.0f)
        {
            //switch potion up
            SelectPotion(true);
        }

        if (Input.mouseScrollDelta.y < 0.0f)
        {
            //switch potion down
            SelectPotion(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ThrowPotion();
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            UsePotion();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            knockbackForce = 14f;
            if (other.transform.parent == null)
            {
                ApplyKnockBack(other.transform.position);
                TakeDamage(1);
            }
            else
            {
                ApplyKnockBack(other.transform.parent.position);
                TakeDamage(1);
            }
            knockbackForce = 7f;
            
        }
    }
    

    //PLAYER STATS METHODS
    
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (isTough)
        {
            currentHealth++;
        }
    }

    protected override void Die()
    {
        if (currentHealth <= 0)
        {
            //Show game over screen
            transform.position = new Vector3(0, 0, 0);
            currentHealth = maxHealth;
        }
    }

    //PLAYER MOVEMENT METHODS

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
            Vector3 movementDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput);
            
            if (isGrounded)
            {
                characterController.Move(movementDirection * (movementSpeed * Time.deltaTime));
                animator.SetBool("isMoving", true);
            }
            else
            {
                characterController.Move(movementDirection * (movementSpeed * airSpeedMultiplier * Time.deltaTime));
                animator.SetBool("isMoving", false);
            }
        }
    }
    
    protected override void VerticalMovement()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            velocity.y = jumpForce;
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && canJump && canDoubleJump)
        {
            velocity.y = jumpForce;
            canJump = false;
            canDoubleJump = false;
            StartCoroutine(JumpCooldown());
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
            if (velocity.y < -gravity)
            {
                velocity.y = -gravity;
            }
        }
        
        characterController.Move(velocity * Time.deltaTime);
    }

    //PLAYER POTION METHODS

    private void UsePotion()
    {
        if(isDrinking)return;
        if (selectedPotion.PotionAmount > 0)
        {
            Potion drinkPotion = Instantiate(selectedPotion.PotionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            drinkPotion.DrinkEffect(this);
            selectedPotion.PotionAmount--;
            isDrinking = true;
            StartCoroutine(DrinkingCooldown());
        }
    }

    private void ThrowPotion()
    {
        if (selectedPotion.PotionAmount > 0)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 throwDirection = (hit.point - potionThrowSpawn.position).normalized;
                Potion thrownPotion = Instantiate(selectedPotion.PotionPrefab, potionThrowSpawn.position, Quaternion.identity);
                Rigidbody potionRb = thrownPotion.Rb;
                if (potionRb != null)
                {
                    potionRb.AddForce(throwDirection * 30f, ForceMode.Impulse);
                }
            }
            else
            {
                Vector3 throwDirection = (ray.direction).normalized;
                Potion thrownPotion = Instantiate(selectedPotion.PotionPrefab, potionThrowSpawn.position, Quaternion.identity);
                Rigidbody potionRb = thrownPotion.Rb;
                if (potionRb != null)
                {
                    potionRb.AddForce(throwDirection * 30f, ForceMode.Impulse);
                }
            }

            selectedPotion.PotionAmount--;
        }
    }

    private void SelectPotion(bool up)
    {
        if (selectedPotion != null)
        {
            if (up)
            {
                int currentPos = Array.IndexOf(potionObjects, selectedPotion);
                if (currentPos + 1 > potionObjects.Length - 1)
                {
                    selectedPotion = potionObjects[0];
                }
                else
                {
                    selectedPotion = potionObjects[currentPos + 1];
                }
            }
            else
            {
                int currentPos = Array.IndexOf(potionObjects, selectedPotion);
                if (currentPos - 1 < 0)
                {
                    selectedPotion = potionObjects[^1];
                }
                else
                {
                    selectedPotion = potionObjects[currentPos - 1];
                }
            }
        }
        else
        {
            selectedPotion = potionObjects[0];
        }
    }

    private IEnumerator DrinkingCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isDrinking = false;
    }

    private IEnumerator GetClosestInteractableObject()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(1);
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            if (colliders.Length > 0)
            {
                GameObject closestObject = colliders
                    .Where(collider => collider.gameObject != null && collider.gameObject != gameObject)
                    .Where(collider => collider.GetComponent<InteractableObjectSuper>() != null)
                    .OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position))
                    .Select(collider => collider.gameObject)
                    .FirstOrDefault();
                if (closestObject != null)
                {
                    InteractableObjectSuper closestInteractableObject = closestObject.GetComponent<InteractableObjectSuper>();

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