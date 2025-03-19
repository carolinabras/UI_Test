using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    Vector2 moveInput;
    Rigidbody2D rb;
    public float moveSpeed = 1f;
    public ContactFilter2D moveFilter;
    public float collisionOffset = 0.05f;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    
    // --------------- animations --------------- //
    public Animator animator;
    SpriteRenderer spriteRenderer;
    
    
    // ------------- button mashing ------------- //
    public float mashDelay = 0.5f;
    private bool isPressed;
    private float mashTimer;
    private bool isMashStarted = false;
    public Chest Chest;
    public bool chestInRange = false;
    public float mashTimeRequired = 2.0f;
    public float mashStartTime;
    float elapsedTime;
    public bool mashingSucess = false;
    
    // ------------------ UI ------------------ //
    public pressUIController pressUI;
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //pressUI.deactivateUI();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // ------------- button mashing ------------- //
        mashTimer = mashDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        {
            Debug.Log("P pressed");
        }
        if (!(chestInRange) || Chest.isOpen)
        {
            pressUI.deactivateUI();
            isMashStarted = false;
        }
        else
        {
            pressUI.activateUI();
            
            // mashing initialization
            if (Input.GetKeyDown(KeyCode.P) && !isMashStarted)
            {
                isMashStarted = true;
                mashStartTime = Time.time; // starting real time
                mashTimer = mashDelay; // reset timer
                mashingSucess = false;
                
                Debug.Log("mashing started");
            }

            // if mashing is started (pressed P once)
            if (isMashStarted)
            {
                float elapsedTime = Time.time - mashStartTime; 

                Debug.Log("mashing... " + elapsedTime);

                // if player presses P again, it resets the delay
                if (Input.GetKeyDown(KeyCode.P) || !(chestInRange))
                {
                    mashTimer = mashDelay; 
                }

                // if player presses P quick enough in time required, chest opns
                if (elapsedTime > mashTimeRequired)
                {
                    Debug.Log("chest opened!!!");
                    Chest.OpenChest();
                    pressUI.deactivateUI();
                    ResetMashing();
                    mashingSucess = true;
                    return; 
                }

                // mash failed
                if (mashTimer <= 0 || !(chestInRange))
                {
                    Debug.Log("mash failed :(");
                    ResetMashing();
                }
                
                mashTimer -= Time.deltaTime;
            }
        }
    }
    
    void ResetMashing()
    {
        isMashStarted = false;
        mashStartTime = 0f;
        mashTimer = mashDelay;
    }

    // --------------- movement --------------- //
    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
           bool sucess = TryMove(moveInput);
           
           if (!sucess)
           {
               sucess = TryMove(new Vector2(moveInput.x, 0));
                if (!sucess)
                {
                     sucess = TryMove(new Vector2(0, moveInput.y));
                }
           }
           animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        
    }
    
    void OnMove(InputValue moveValue)
    {
        moveInput = moveValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction)
    {
        //check for collisions before moving
        int count = rb.Cast(
            direction,
            moveFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime + collisionOffset));
            return true;
            
        }
        else
        {
            return false;
        }
    }
    
    
    // --------------- check collisions  --------------- //
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        {
            chestInRange = true;
            pressUI.activateUI();
            Debug.Log("chest in range");
            
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        {
            chestInRange = false;
            pressUI.deactivateUI();
            Debug.Log("chest out of range");
            
        }
    }
} 

