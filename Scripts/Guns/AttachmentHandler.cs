using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentHandler : MonoBehaviour
{
    public Attachment attachmentIdentity;
    public GameObject attachmentObj;

    [Header("Main")]
    public string attachmentName;
    public Attachment.AttachmentType attachmentType;
    public int attachmentID;

    [Header("Stats")]
    public int damageMod;
    public float fireRateMod;
    public float accuracyMod;
    public int armorPenMod;
    public float bulletSpeedMod;
    public int bulletsAtOnceMod;
    
    [Header("Graphics and Prefabs")]
    bool editsBullet;
    public GameObject newBulletPrefab;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Sprite attachmentSprite;
    //public Anim gunShooting

    public bool isAttached;

    void Awake()
    {
        attachmentObj = this.gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public void UpdateAttachments()
    {
        attachmentName = attachmentIdentity._attachmentName;
        attachmentType = attachmentIdentity._attachmentType;
        attachmentID = attachmentIdentity._attachmentID;

        damageMod = attachmentIdentity._damageMod;
        fireRateMod = attachmentIdentity._fireRateMod;
        accuracyMod = attachmentIdentity._accuracyMod;
        armorPenMod = attachmentIdentity._armorPenMod;
        bulletSpeedMod = attachmentIdentity._bulletSpeedMod;
        bulletsAtOnceMod = attachmentIdentity._bulletsAtOnceMod;
    
        editsBullet = attachmentIdentity._editsBullet;
        newBulletPrefab = attachmentIdentity._newBulletPrefab;
        attachmentSprite = attachmentIdentity._attachmentSprite;

        //spriteRenderer.sprite = attachmentSprite;
    }

    public void ClearAttachments()
    {
        attachmentName = "";
        attachmentID = 0;

        damageMod = 0;
        fireRateMod = 0;
        accuracyMod = 0;
        armorPenMod = 0;
        bulletSpeedMod = 0;
        bulletsAtOnceMod = 0;
    
        editsBullet = false;
        newBulletPrefab = null;
        attachmentSprite = null;
    }
}
