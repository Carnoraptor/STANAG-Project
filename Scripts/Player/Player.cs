using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2d;

    float horizontal;
    float vertical;
    //float scaleX = 5;

    //Movement Variables
    public float moveSpeed = 12f;
    public float acceleration = 70;
    public float deacceleration = 50;
    public float currentSpeed = 0;
    public bool isMoving = false;
    public Vector2 movementVector;
    Vector2 direction;

    //Player Variables
    public Transform playerTransform;
    public Vector2 position;
    Animator playerAnimator;
    AudioSource audioSource;

    //Gun Variables
    GameObject gunObj;
    GunHandler gunHandler;

    //Backend Variables
    float scaleX = 1;
    float scaleY = 1;

    //Health Variables
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int playerArmor;

    //public GameObject levelEndUI;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gunObj = GameObject.FindWithTag("Gun");
        gunHandler = gunObj.GetComponent<GunHandler>();
        playerCurrentHealth = playerMaxHealth;
    }

    void Update()
    {
        Move();
        position = transform.position;
    }

    void MovementCalculations()
    {
        /*if (isMoving == true && currentSpeed < moveSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime; 
        }

        if (isMoving == false && currentSpeed > 1.5f)
        {
            currentSpeed -= deacceleration * Time.deltaTime;
        }*/

        //currentSpeed = Mathf.Clamp(currentSpeed, 1f, moveSpeed);
        currentSpeed = moveSpeed;

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        direction = new Vector2(horizontal, vertical).normalized;
    }

    void Move()
    {
        MovementCalculations();
        //Flip(); WORK ON FLIPPING PLAYER AS IT CURRENTLY DISLIKES DOING THAT
        rb2d.velocity = new Vector2(direction.x * currentSpeed, direction.y * currentSpeed);
    }

    void Flip()
    {
        switch (gunHandler.gunDir)
        {
            case 1:
            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
            break;

            case -1:
            transform.localScale = new Vector3(scaleX * (-1), scaleY, transform.localScale.z);
            break;
        }
    }

    //Health Functions
    public void TakeDamage(int damage, int armorPierce)
    {
        Debug.Log("Damage Taken!");
        int armorLeft = playerArmor - armorPierce;
        if (armorLeft < 0)
        {
            armorLeft = 0;
        }
        int damagePassed = damage - armorLeft;
        if (damagePassed < 0)
        {
            damagePassed = 0;
        }

        playerCurrentHealth -= damagePassed;
    }

    public void PlayerDeath()
    {
        Debug.Log("deadass ngl");
    }
}
