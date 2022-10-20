using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAI : MonoBehaviour
{
    public EnemyAI ai;

    [Header("Main")]
    public string enemyName;
    public EnemyAI.EnemyType enemyType;
    public int enemyID;
    //public var enemyBehaviour;

    [Header("Stats")]
    public int enemyHP;
    public int enemyArmor;
    public int enemyDamage;
    public int enemyArmorPierce;
    public int enemySpeed;
    public int enemyAttackRate;
    public int enemyCurrentHealth;

    [Header("Graphics and Prefabs")]
    public Sprite enemySprite;

    //Universal Backends (irrelevant to ScriptableObject)
    [Header("Backends")]
    [HideInInspector] public bool isDead;
    float scaleX;
    [HideInInspector] public int direction;
    [HideInInspector] public bool doMovement = true;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public GameObject playerObj;
    [HideInInspector] public Player playerScript;

    [HideInInspector] Vector2 targetPos;
    [HideInInspector] Vector2 thisPos;
    [HideInInspector] public float angleToPlayer;

    Vector2 startingPosition;
    public GameObject dummy;
    

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag="Enemy"; 
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();

        //Main
        enemyName = ai._enemyName;
        enemyType = ai._enemyType;
        enemyID = ai._enemyID;
        //Stats
        enemyHP = ai._enemyHP;
        enemyArmor = ai._enemyArmor;
        enemyDamage = ai._enemyDamage;
        enemyArmorPierce = ai._enemyArmorPierce;
        enemySpeed = ai._enemySpeed;
        enemyAttackRate = ai._enemyAttackRate;
        enemyCurrentHealth = enemyHP;
        //Graphics & Prefabs
        enemySprite = ai._enemySprite;

        startingPosition = transform.position;
    }

    void Update()
    {
        if (isDead == true)
        {
            Color tmp = spriteRenderer.color;
            tmp.a -= Time.deltaTime;
            spriteRenderer.color = tmp;
            StartCoroutine(RespawnDummy());
        }
    }

    void FixedUpdate()
    {
        if (doMovement)
        {
            Movement();
        }
    }

    void FindPlayer()
    {
        targetPos = playerObj.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.x;
        angleToPlayer = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToPlayer));
    }

    void Movement()
    {
        //transform.right = playerObj.transform.position - transform.position;

        //transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, enemySpeed * Time.deltaTime);
        Flip();
    }

    void Flip()
    {
        scaleX = transform.localScale.x;

        if (transform.rotation.z <= 90f || transform.rotation.z >= -90f)
        {
            direction = 1;
        }
        else 
        {
            direction = -1;
        }

        if (direction == 1)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipped right... or tried to");
        }
        else if (direction == -1)
        {
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipped left... or tried to");
        }
    }

    //ROAMING
    
    //DAMAGE & HEALTH FUNCTIONS

    public void DealDamage()
    {
        playerScript.TakeDamage(enemyDamage, enemyArmorPierce);
    }

    public void TakeDamage(int damage, int armorPierce)
    {
        int armorLeft = enemyArmor - armorPierce;
        if (armorLeft < 0)
        {
            armorLeft = 0;
        }
        //Debug.Log(enemyName + " has " + armorLeft + " armor unpierced");
        int damagePassed = damage - armorLeft;
        if (damagePassed < 0)
        {
            damagePassed = 0;
        }
        //Debug.Log(enemyName + " takes " + damagePassed + " damage"); 
        enemyCurrentHealth -= damagePassed;
        if (enemyCurrentHealth <= 0)
        {
            enemyCurrentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        //Debug.Log(enemyName + " died!");
        isDead = true;
    }

    IEnumerator RespawnDummy()
    {
        yield return new WaitForSeconds(5f);
        GameObject newDummy = Instantiate(dummy, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}

