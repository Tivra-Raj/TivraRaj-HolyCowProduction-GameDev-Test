using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : Character
{
    [SerializeField]
    float offset;
    [SerializeField]
    private Rigidbody2D playerRigidbody;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashCoolDown;
    [SerializeField]
    float dashTime;
    [SerializeField]
    GameObject healthUI;
    [SerializeField]
    SpriteRenderer playerSprite;
    [SerializeField]
    TrailRenderer trail;

    float horizontalMovement;
    float verticalMovement;
    Vector2 movement;

    GameManager gameManager;

    public bool canMove=true;
    bool canDash = true;
    bool isDashing = false;
    bool isPulsing=false;
    private int health = 1;

    public override void Start()
    {
        base.Start();
        isPlayer = true;
        UpdateHealthUI(currentHP);
        trail.emitting = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove)
        {
            HandleInput();
        }
    }

    private void UpdateHealthUI(int health)
    {
        int children = healthUI.transform.childCount;
        for(int i=0;i<children;i++)
        {
            Transform child = healthUI.transform.GetChild(i);
            if (i < health)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    void FixedUpdate()
    {
        if (canMove && !isDashing)
        {
            MovePlayer();
        }
    }

    private void HandleInput()
    {
        //Movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontalMovement, verticalMovement).normalized;
        //Dash
        if(Input.GetKeyDown(KeyCode.LeftShift)&&canDash)
        {
            StartCoroutine(Dash());
        }/*
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire(shootpoint,attackRange,isPlayer);
        }*/
    }

    private void MovePlayer()
    {
        if(playerRigidbody != null)
        {
            playerRigidbody.velocity = new Vector2(movement.x, movement.y) * moveSpeed;  
        }
        else
        {
            Debug.LogError("Rigidbody is not assigned!");
        }
        /*if (movement != Vector2.zero)
        {
            float rotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation + offset);
        }*/
    }
    
    private IEnumerator Dash()
    {
        //TODO Dash audio
        isDashing = true;
        canDash = false;
        trail.emitting = true;
        playerRigidbody.velocity = new Vector2(movement.x * dashSpeed, movement.y * dashSpeed);
        yield return new WaitForSeconds(dashTime);
        isDashing=false;
        trail.emitting = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(shootpoint.position, attackRange);
    }

    public override void ApplyDamage(int damage)
    {
        //TODO Player takes damage audio
        base.ApplyDamage(damage);
        UpdateHealthUI(currentHP);
        if(!isPulsing)
        {
            StartCoroutine(ShowHitImpact());
        }
        
    }
    IEnumerator ShowHitImpact()
    {
        isPulsing = true;
        Color spriteColor = playerSprite.color;
        for (int i = 0; i < 2; i++)
        {
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = spriteColor;
            yield return new WaitForSeconds(0.1f);
        }
        isPulsing = false;
    }
    public override void Die()
    {
        //TODO Player Death audio
        base.Die();
        if(gameManager != null)
        {
            gameManager.ShowLoseScreen();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Knockback
        if (collision.collider.CompareTag(Constants.patrolTag))
        {
            ApplyDamage(1);
        }
    }

}
