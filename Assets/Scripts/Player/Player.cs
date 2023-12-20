using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Player : CharacterSuper
{
    public static Player Instance;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform camera;
    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private int freeLookXAxis;
    [SerializeField] private int freeLookYAxis;
    [SerializeField] private PotionObject[] potionObjects;
    [SerializeField] private Transform potionThrowSpawn;
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private Vector3 throwDirection;
    private PotionObject selectedPotion;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float groundDrag = 0.2f;
    private float interactRange = 3f;
    private InteractableObjectSuper interactableObjectSuper;
    private bool isAiming;
    private bool isCrafting;
    private bool inventoryActive;

    public PotionObject SelectedPotion => selectedPotion;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SelectPotion(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //load file
        StartCoroutine(GetClosestInteractableObject());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!Inventory.Instance.IsActive)
            {
                Inventory.Instance.OpenInventory();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                freeLook.m_YAxis.m_MaxSpeed = 0;
                freeLook.m_XAxis.m_MaxSpeed = 0;
                freeLook.gameObject.SetActive(true);
                aimCamera.gameObject.SetActive(false);
                isAiming = false;
                inventoryActive = true;
            }
            else
            {
                Inventory.Instance.CloseInventory();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                freeLook.m_YAxis.m_MaxSpeed = freeLookYAxis;
                freeLook.m_XAxis.m_MaxSpeed = freeLookXAxis;
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
        
        if(inventoryActive)return;
        if(isStunned)return;

        CameraRotation();

        Aim();

        Move();

        if (Input.GetAxis("Jump") != 0 && isGrounded && canJump)
        {
            Jump();
            if (canDoubleJump)
            {
                canDoubleJump = false;
                return;
            }

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

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ThrowPotion();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Melee();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            UsePotion();
        }
    }

    //PLAYER STATS METHODS

    private void SetPlayerStats(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            //DIE
        }
    }

    //PLAYER CAMERA METHODS

    private void CameraRotation()
    {
        if (isAiming)
        {
            
        }
        else
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
    }

    private void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            Vector3 lookDirection = (ray.direction).normalized;
            Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);

            freeLook.gameObject.SetActive(false);
            aimCamera.gameObject.SetActive(true);
            isAiming = true;
        }
        else
        {
            freeLook.gameObject.SetActive(true);
            aimCamera.gameObject.SetActive(false);
            isAiming = false;
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

            characterController.Move(movementDirection * (movementSpeed * Time.deltaTime));

            if (movementDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            }
            
            /*
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
            */
            if (isGrounded)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            
        }
    }

    private void Jump()
    {
        
        animator.SetBool("isJumping", true);
        
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }


    private void Melee()
    {
        if (isPassiv) return;
    }

    private void KnockBack()
    {
        if (isTough) return;
    }

    //PLAYER POTION METHODS

    private void UsePotion()
    {
        if (selectedPotion.PotionAmount > 0)
        {
            selectedPotion.PotionPrefab.DrinkEffect(this);
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
                Rigidbody potionRb = thrownPotion.GetComponent<Rigidbody>();
                if (potionRb != null)
                {
                    potionRb.AddForce(throwDirection * 30f, ForceMode.Impulse);
                    Debug.Log("throw");
                }
            }
            else
            {
                Vector3 throwDirection = (ray.direction).normalized;
                Potion thrownPotion = Instantiate(selectedPotion.PotionPrefab, potionThrowSpawn.position, Quaternion.identity);
                Rigidbody potionRb = thrownPotion.GetComponent<Rigidbody>();
                if (potionRb != null)
                {
                    potionRb.AddForce(throwDirection * 30f, ForceMode.Impulse);
                    Debug.Log("throw");
                }
            }

            selectedPotion.PotionAmount--;
        }
        else
        {
            Melee();
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