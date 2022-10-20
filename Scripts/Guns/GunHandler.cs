using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public Gun currentGun;
    public Bullet currentBullet;

    [Header("Gun")]
    [Header("Main")]
    public string gunName;
    public Gun.GunType gunType;

    [Header("Stats")]
    public int damage;
    public float fireRate;
    public float inaccuracy;
    public int armorPen;
    public float bulletSpeed;
    public int bulletsAtOnce;
    public Gun.FireMode fireMode;

    [Header("Graphics and Prefabs")]
    public GameObject bulletPrefab;
    public Sprite gunSprite;
    public Vector2 bulletOriginOffset;
    private GameObject bulletOriginObj;
    public Transform bulletOrigin;
    public Transform gunPos;
    public float radius;
    SpriteRenderer spriteRenderer;
    public GameObject attachmentPrefab;
    //Dropped Object prefabs
    public GameObject droppedGun;
    public GameObject droppedAttachment;

    [Header("Attachments")]
    //public List<GameObject> currentAttachmentObjs = new List<GameObject>();
    public List<Attachment> currentAttachments = new List<Attachment>();
    public List<AttachmentHandler> currentAttachmentHandlers = new List<AttachmentHandler>();
    Vector2 muzzleLoc, opticLoc, lowerRailLoc, sideRailLoc;
    bool hasMuzzleSlot, hasOpticSlot, hasLowerRailSlot, hasSideRailSlot;
    [HideInInspector] public AttachmentHandler muzzleHandler, opticHandler, LRHandler, SRHandler;

    [Header("Backend Variables")]
    //Gun Backend
    Rigidbody2D rb2d;
    public int gunDir; //Direction of gun
    float scaleX = 1;
    float scaleY = 1;
    bool canShoot = true;

    //Universal Backends
    public Vector2 cursorPos;
    Camera cam;
    public float normalZoom = 4f;

    //Inventory Backends
    public bool canPickUpGun = true;
    public bool canPickUpAttachment = true;

    //Player Backends
    GameObject playerObj;
    Vector2 playerPos;
    Player playerScript;


    

//░██████╗████████╗░█████╗░██████╗░████████╗ 
//██╔════╝╚══██╔══╝██╔══██╗██╔══██╗╚══██╔══╝ 
//╚█████╗░░░░██║░░░███████║██████╔╝░░░██║░░░ 
//░╚═══██╗░░░██║░░░██╔══██║██╔══██╗░░░██║░░░ 
//██████╔╝░░░██║░░░██║░░██║██║░░██║░░░██║░░░ 
//╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░ 

    void Start()
    {
        //Gun Assignments
        gunName = currentGun._gunName;
        gunType = currentGun._gunType; 

        //Base Stat Assignments
        damage = currentGun._damage; 
        fireRate = currentGun._fireRate; 
        inaccuracy = currentGun._inaccuracy; 
        armorPen = currentGun._armorPen; 
        bulletSpeed = currentGun._bulletSpeed;
        bulletsAtOnce = currentGun._bulletsAtOnce;
        fireMode = currentGun._fireMode;

        //Graphics/Prefabs Assignments
        bulletPrefab = currentGun._bulletPrefab;
        gunSprite = currentGun._gunSprite;
        bulletOriginOffset = currentGun._bulletOriginOffset;
        radius = currentGun._radius;
        bulletOriginObj = GameObject.Find("BulletOrigin");
        bulletOrigin = bulletOriginObj.transform;
        gunPos = this.gameObject.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = gunSprite;
        //Add in the bullet origin's position being modified by the gun's + offset, including direction modifiers

        //Attachment Assignments
        muzzleLoc = currentGun._muzzleLoc;
        opticLoc = currentGun._opticLoc;
        lowerRailLoc = currentGun._lowerRailLoc;
        sideRailLoc = currentGun._sideRailLoc;

        hasMuzzleSlot = currentGun._hasMuzzleSlot;
        hasOpticSlot = currentGun._hasOpticSlot;
        hasLowerRailSlot = currentGun._hasLowerRailSlot;
        hasSideRailSlot = currentGun._hasSideRailSlot;

        GameObject[] allAttachments = GameObject.FindGameObjectsWithTag("Attachment");
        foreach (GameObject g in allAttachments)
        {
            //Here for later convenience
        }

        muzzleHandler = GameObject.Find("Muzzle Handler").GetComponent<AttachmentHandler>();
        opticHandler = GameObject.Find("Optic Handler").GetComponent<AttachmentHandler>();
        LRHandler = GameObject.Find("Lower Rail Handler").GetComponent<AttachmentHandler>();
        SRHandler = GameObject.Find("Side Rail Handler").GetComponent<AttachmentHandler>();
        currentAttachmentHandlers.Add(muzzleHandler); 
        currentAttachmentHandlers.Add(opticHandler); 
        currentAttachmentHandlers.Add(LRHandler); 
        currentAttachmentHandlers.Add(SRHandler); 

        //Backend Assignments
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();


        UpdateGun();
    }

//██╗░░░██╗██████╗░██████╗░░█████╗░████████╗███████╗
//██║░░░██║██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔════╝
//██║░░░██║██████╔╝██║░░██║███████║░░░██║░░░█████╗░░
//██║░░░██║██╔═══╝░██║░░██║██╔══██║░░░██║░░░██╔══╝░░
//╚██████╔╝██║░░░░░██████╔╝██║░░██║░░░██║░░░███████╗
//░╚═════╝░╚═╝░░░░░╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝

    void Update()
    {
        //Constant Updating Assignments and Functions
        Vector2 playerPos = playerScript.position;
        //Looking towards mouse
        LookAtCursor();
        //Flipping sprite based on X
        Flip();

        //Code to make the gun rotate on a set track around the player
        Vector2 playerToCursor = cursorPos - playerPos;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * radius;

        transform.position = playerPos + cursorVector;

        switch(fireMode)
        {
            case Gun.FireMode.Automatic:
            if (Input.GetButton("Fire1") && canShoot)
            {
                Shoot();
            }
            break;
            case Gun.FireMode.SemiAutomatic:
            if (Input.GetButton("Fire1") && canShoot)
            {
                Shoot();
            }
            break;
        }
    }


//██╗███╗░░██╗██╗░░░██╗███████╗███╗░░██╗████████╗░█████╗░██████╗░██╗░░░██╗
//██║████╗░██║██║░░░██║██╔════╝████╗░██║╚══██╔══╝██╔══██╗██╔══██╗╚██╗░██╔╝
//██║██╔██╗██║╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░██║░░██║██████╔╝░╚████╔╝░
//██║██║╚████║░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░██║░░██║██╔══██╗░░╚██╔╝░░
//██║██║░╚███║░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░╚█████╔╝██║░░██║░░░██║░░░
//╚═╝╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝░░░╚═╝░░░

    public void UpdateGun()
    {
        //Gun Assignments
        gunName = currentGun._gunName;
        gunType = currentGun._gunType; 

        //Base Stat Assignments
        damage = currentGun._damage; 
        fireRate = currentGun._fireRate; 
        inaccuracy = currentGun._inaccuracy; 
        armorPen = currentGun._armorPen; 
        bulletSpeed = currentGun._bulletSpeed;

        //Graphics/Prefabs Assignments
        bulletPrefab = currentGun._bulletPrefab;
        gunSprite = currentGun._gunSprite;
        bulletOriginOffset = currentGun._bulletOriginOffset;
        radius = currentGun._radius;
        bulletsAtOnce = currentGun._bulletsAtOnce;
        bulletOriginObj = GameObject.Find("BulletOrigin");
        bulletOrigin = bulletOriginObj.transform;
        gunPos = this.gameObject.transform;

        //Attachment Assignments
        muzzleLoc = currentGun._muzzleLoc;
        opticLoc = currentGun._opticLoc;
        lowerRailLoc = currentGun._lowerRailLoc;
        sideRailLoc = currentGun._sideRailLoc;

        hasMuzzleSlot = currentGun._hasMuzzleSlot;
        hasOpticSlot = currentGun._hasOpticSlot;
        hasLowerRailSlot = currentGun._hasLowerRailSlot;
        hasSideRailSlot = currentGun._hasSideRailSlot;

        //Backend Assignments
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();;
        spriteRenderer.sprite = gunSprite;

        //Bullet Origin Adjustment
        Quaternion quat = gunPos.rotation;
        gunPos.rotation = Quaternion.identity;
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
        Vector2 desiredGunPos;
        desiredGunPos.x = gunPos.position.x;
        desiredGunPos.y = gunPos.position.y;
        Vector2 BODesPos = desiredGunPos + bulletOriginOffset;
        float desiredY = bulletOriginOffset.y;
        if (desiredY != bulletOriginOffset.y)
        {
            Vector2 absBOPos;
            absBOPos.x = BODesPos.x;
            absBOPos.y = desiredY;
            bulletOrigin.position = absBOPos;
            //Debug.Log("Adjusted Y pos");
        }
        else
        {
            bulletOrigin.position = BODesPos;
        }
        gunPos.rotation = quat;

        //Cam Specific Adjustments
        if (cam.orthographicSize > normalZoom)
        {
            ZoomCamIn(normalZoom, 0.02f);
        }
        else if (cam.orthographicSize < normalZoom)
        {
            ZoomCamOut(normalZoom, 0.02f);
        }

        if (gunType == Gun.GunType.DMR)
        {
            Debug.Log("Zoomin out");
            ZoomCamOut((normalZoom * 1.5f), 0.02f);
        }

        UpdateAttachments(null);
    }

    public void SwitchGun(int switchedToGun)
    {
        
        UpdateGun();
    }

    //Gun Dropping/Picking Up
    public void PlayerPickUpGun(Gun newGun, DroppedGun dG)
    {
        canPickUpGun = false;
        StartCoroutine(PickupCooldown(0));
        PlayerDropGun(currentGun);
        currentGun = newGun;
        UpdateGun();
        Destroy(dG.thisObj);
    }    

    public void PlayerDropGun(Gun gunToDrop)
    {
        GameObject gunDroppedObj = Instantiate(droppedGun, gunPos.position, Quaternion.identity);
        Rigidbody2D gunRB = gunDroppedObj.GetComponent<Rigidbody2D>();
        Vector2 gunDropDir;
        gunDropDir.x = Mathf.Clamp(cursorPos.x, -3, 3);
        gunDropDir.y = Mathf.Clamp(cursorPos.y, -3, 3);
        gunRB.AddForce(gunDropDir * 2, ForceMode2D.Impulse);
        DroppedGun dG = gunDroppedObj.GetComponent<DroppedGun>();
        dG.GenerateIdentity(gunToDrop);
    }

    //Attachment Dropping/Picking Up
    public void PlayerPickUpAttachment(Attachment newAttachment, DroppedAttachment dA)
    {
        canPickUpAttachment = false;
        StartCoroutine(PickupCooldown(1));
        UpdateAttachments(newAttachment);
        Destroy(dA.thisObj);
    }    

    public void PlayerDropAttachment(Attachment attachmentToDrop)
    {
        GameObject attachmentDroppedObj = Instantiate(droppedAttachment, gunPos.position, Quaternion.identity);
        Rigidbody2D atchRB = attachmentDroppedObj.GetComponent<Rigidbody2D>();
        Vector2 gunDropDir;
        gunDropDir.x = Mathf.Clamp(cursorPos.x, -3, 3);
        gunDropDir.y = Mathf.Clamp(cursorPos.y, -3, 3);
        atchRB.AddForce(gunDropDir * 2, ForceMode2D.Impulse);
        DroppedAttachment dA = attachmentDroppedObj.GetComponent<DroppedAttachment>();
        dA.GenerateIdentity(attachmentToDrop);
        switch(attachmentToDrop._attachmentType)
        {
            case Attachment.AttachmentType.muzzle:
            muzzleHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.optic:
            opticHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.lowerRail:
            LRHandler.attachmentIdentity = null;
            break;
            case Attachment.AttachmentType.sideRail:
            SRHandler.attachmentIdentity = null;
            break;
        }
        //UpdateGun();
    }

    IEnumerator PickupCooldown(int PickupIdentity) //1 is attachment, 0 is gun
    {
        yield return new WaitForSeconds(2f);
        switch(PickupIdentity)
        {
            case 0:
            canPickUpGun = true;
            break;
            case 1:
            canPickUpAttachment = true;
            break;
        }
    }
    


//░█████╗░░█████╗░░██████╗███╗░░░███╗███████╗████████╗██╗░█████╗░░██████╗
//██╔══██╗██╔══██╗██╔════╝████╗░████║██╔════╝╚══██╔══╝██║██╔══██╗██╔════╝
//██║░░╚═╝██║░░██║╚█████╗░██╔████╔██║█████╗░░░░░██║░░░██║██║░░╚═╝╚█████╗░
//██║░░██╗██║░░██║░╚═══██╗██║╚██╔╝██║██╔══╝░░░░░██║░░░██║██║░░██╗░╚═══██╗
//╚█████╔╝╚█████╔╝██████╔╝██║░╚═╝░██║███████╗░░░██║░░░██║╚█████╔╝██████╔╝
//░╚════╝░░╚════╝░╚═════╝░╚═╝░░░░░╚═╝╚══════╝░░░╚═╝░░░╚═╝░╚════╝░╚═════╝░

    void LookAtCursor()
    {
        cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = cursorPos - rb2d.position;
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg /*- 90f*/;
        rb2d.rotation = angle;
    }

    void GunPositioning()
    {
        //Code to make the gun rotate on a set track around the player
        Vector2 playerToCursor = cursorPos - playerPos;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * radius;

        if (playerToCursor.magnitude < cursorVector.magnitude) // detect if mouse is in inner radius
            {cursorVector = playerToCursor;}

        transform.position = playerPos + cursorVector;
    }

    void Flip()
    {
        if (gunPos.position.x > playerScript.position.x)
        {
            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
            gunDir = 1;
        }
        else
        {
            transform.localScale = new Vector3(scaleX, (-1) * scaleY, transform.localScale.z);
            gunDir = -1;
        }
    }

//    
//░██████╗░░█████╗░███╗░░░███╗███████╗██████╗░██╗░░░░░░█████╗░██╗░░░██╗
//██╔════╝░██╔══██╗████╗░████║██╔════╝██╔══██╗██║░░░░░██╔══██╗╚██╗░██╔╝
//██║░░██╗░███████║██╔████╔██║█████╗░░██████╔╝██║░░░░░███████║░╚████╔╝░
//██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░██╔═══╝░██║░░░░░██╔══██║░░╚██╔╝░░
//╚██████╔╝██║░░██║██║░╚═╝░██║███████╗██║░░░░░███████╗██║░░██║░░░██║░░░
//░╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝╚═╝░░░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░
    
    void Shoot()
    {
        //Generates inaccuracy dynamically
        //Creates a bullet, gets it's rigidbody
        for (int b = 0; b<bulletsAtOnce; b++)
        {
            RandomAngle();
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            //Shoots the bullet
            bulletRB.AddForce(bulletOrigin.right * bulletSpeed, ForceMode2D.Impulse);
        }
        StartCoroutine(FireRateCheck());
    }
    //Notes: Add a 'foreach bulletsAtOnce in bulletsAtOnce' or whatever, just to make it so shotguns can be added just as easily as other guns without too much code jigging

    IEnumerator FireRateCheck()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }


//░█████╗░░█████╗░███╗░░██╗██╗░░░██╗███████╗███╗░░██╗██╗███████╗███╗░░██╗░█████╗░███████╗
//██╔══██╗██╔══██╗████╗░██║██║░░░██║██╔════╝████╗░██║██║██╔════╝████╗░██║██╔══██╗██╔════╝
//██║░░╚═╝██║░░██║██╔██╗██║╚██╗░██╔╝█████╗░░██╔██╗██║██║█████╗░░██╔██╗██║██║░░╚═╝█████╗░░
//██║░░██╗██║░░██║██║╚████║░╚████╔╝░██╔══╝░░██║╚████║██║██╔══╝░░██║╚████║██║░░██╗██╔══╝░░
//╚█████╔╝╚█████╔╝██║░╚███║░░╚██╔╝░░███████╗██║░╚███║██║███████╗██║░╚███║╚█████╔╝███████╗
//░╚════╝░░╚════╝░╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝╚═╝╚══════╝╚═╝░░╚══╝░╚════╝░╚══════╝


    //Generates angle variation on bullet origin to simulate gun inaccuracy (stops everything from being a laser beam)
    public void RandomAngle()
    {
        Vector3 vector;

        vector.x = transform.localRotation.x;
        vector.y = transform.localRotation.y;
        vector.z = transform.localRotation.z;
        float minRot = transform.rotation.z + (inaccuracy * -1f);
        float maxRot = transform.rotation.z + inaccuracy;
        float randomNum = Random.Range(minRot, maxRot);
        
        if (gunDir > 0f)
        {
            vector.z = transform.rotation.z + randomNum;
        }
        else
        {
            vector.z = transform.rotation.z - randomNum;
        }
        
        Quaternion quaternion = Quaternion.Euler(vector);

        bulletOrigin.transform.localRotation = quaternion;
    }

    //Make the zooms apply only once per frame

    public void ZoomCamOut(float desCamSize, float zoomSpeed)
    {
        while (cam.orthographicSize < desCamSize)
        {
            float newSize = Mathf.MoveTowards(cam.orthographicSize, desCamSize, zoomSpeed * Time.deltaTime);
            cam.orthographicSize = newSize;
        }
    }

    public void ZoomCamIn(float desCamSize, float zoomSpeed)
    {
        while (cam.orthographicSize > desCamSize)
        {
            float newSize = Mathf.MoveTowards(cam.orthographicSize, desCamSize, zoomSpeed * Time.deltaTime);
            cam.orthographicSize = newSize;
        }
    }

//    
//░█████╗░████████╗████████╗░█████╗░░█████╗░██╗░░██╗███╗░░░███╗███████╗███╗░░██╗████████╗░██████╗
//██╔══██╗╚══██╔══╝╚══██╔══╝██╔══██╗██╔══██╗██║░░██║████╗░████║██╔════╝████╗░██║╚══██╔══╝██╔════╝
//███████║░░░██║░░░░░░██║░░░███████║██║░░╚═╝███████║██╔████╔██║█████╗░░██╔██╗██║░░░██║░░░╚█████╗░
//██╔══██║░░░██║░░░░░░██║░░░██╔══██║██║░░██╗██╔══██║██║╚██╔╝██║██╔══╝░░██║╚████║░░░██║░░░░╚═══██╗
//██║░░██║░░░██║░░░░░░██║░░░██║░░██║╚█████╔╝██║░░██║██║░╚═╝░██║███████╗██║░╚███║░░░██║░░░██████╔╝
//╚═╝░░╚═╝░░░╚═╝░░░░░░╚═╝░░░╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝╚═╝░░╚══╝░░░╚═╝░░░╚═════╝░

public void UpdateAttachments(Attachment newAttachmentToAdd) //call with 'null' if no new attachments
    {
        currentAttachments.Clear();
        foreach (AttachmentHandler a in currentAttachmentHandlers)
        {
            if (a.attachmentIdentity != null)
            {
                currentAttachments.Add(a.attachmentIdentity);
            }
        }
        if (newAttachmentToAdd != null)
        {
            currentAttachments.Add(newAttachmentToAdd);
        }

        foreach (Attachment atch in currentAttachments)
        {
            switch (atch._attachmentType)
            {
                case Attachment.AttachmentType.muzzle:
                muzzleHandler.attachmentIdentity = atch;
                break;

                case Attachment.AttachmentType.optic:
                opticHandler.attachmentIdentity = atch;
                break;

                case Attachment.AttachmentType.lowerRail:
                LRHandler.attachmentIdentity = atch;
                break;
                
                case Attachment.AttachmentType.sideRail:
                SRHandler.attachmentIdentity = atch;
                break;
            }
        }
        

        foreach (AttachmentHandler a in currentAttachmentHandlers)
        {
            //Calculate Location On Gun
            Quaternion quat = gunPos.rotation;
            gunPos.rotation = Quaternion.identity;
            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);

            //Desired gun position
            Vector2 desiredGunPos;
            desiredGunPos.x = gunPos.position.x;
            desiredGunPos.y = gunPos.position.y;

            //Variables for the following section (assigned to avoid CS0165)
            Vector2 attachmentLoc = Vector2.zero;
            Vector2 atchDesPos = Vector2.zero;
            Vector2 absAtchPos = Vector2.zero;

            if (a.attachmentIdentity == null)
            {
                a.ClearAttachments();
            }
            else
            {
                a.UpdateAttachments();
            }

            //Checking attachment type and the desired attachment location
            switch (a.attachmentType)
            {
                case Attachment.AttachmentType.muzzle:
                if (hasMuzzleSlot)
                {
                attachmentLoc = currentGun._muzzleLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                muzzleHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.muzzle)
                {
                    muzzleHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (muzzleHandler.attachmentIdentity != null)
                {
                    muzzleHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;

                case Attachment.AttachmentType.optic:
                if (hasOpticSlot)
                {
                attachmentLoc = currentGun._opticLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                opticHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.optic)
                {
                    opticHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (opticHandler.attachmentIdentity != null)
                {
                    opticHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;

                case Attachment.AttachmentType.lowerRail:
                if (hasLowerRailSlot)
                {
                attachmentLoc = currentGun._lowerRailLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                LRHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.lowerRail)
                {
                    LRHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (LRHandler.attachmentIdentity != null)
                {
                    LRHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;
                
                case Attachment.AttachmentType.sideRail:
                if (hasSideRailSlot)
                {
                attachmentLoc = currentGun._sideRailLoc;
                atchDesPos = desiredGunPos + attachmentLoc;
                SRHandler.attachmentObj.transform.position = atchDesPos;
                if (newAttachmentToAdd != null && newAttachmentToAdd._attachmentType == Attachment.AttachmentType.sideRail)
                {
                    SRHandler.attachmentIdentity = newAttachmentToAdd;
                }
                if (SRHandler.attachmentIdentity != null)
                {
                    SRHandler.UpdateAttachments();
                }
                }
                else
                {
                    if (a.attachmentIdentity != null)
                    {
                        PlayerDropAttachment(a.attachmentIdentity);
                        a.attachmentIdentity = null;
                    }
                }
                break;
            }

            gunPos.rotation = quat;
        
            //Clear Empty Attachments & update new ones
            if (a.attachmentIdentity == null)
            {
                a.ClearAttachments();
            }
            else
            {
                a.UpdateAttachments();
            }

            //Apply Modifiers
            if (a.damageMod != 0)
            {
                damage += a.damageMod;
            }
            if (a.fireRateMod != 0)
            {
                fireRate += a.fireRateMod;
            }
            if (a.accuracyMod != 0)
            {
                inaccuracy += a.accuracyMod;
            }
            if (a.armorPenMod != 0)
            {
                armorPen += a.armorPenMod;
            }
            if (a.bulletSpeedMod != 0)
            {
                bulletSpeed += a.bulletSpeedMod;
            }
            if (a.bulletsAtOnceMod != 0)
            {
                bulletsAtOnce += a.bulletsAtOnceMod;
            }

            //Apply Cosmetics
            if (a.attachmentIdentity != null)
            {
                a.spriteRenderer.sprite = a.attachmentSprite;
                a.UpdateAttachments();
            }
            else
            {
                a.spriteRenderer.sprite = null;
            }
        }
    }
}

