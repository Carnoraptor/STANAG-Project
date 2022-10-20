using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public GameObject gun; //The gun object
    public GunHandler gunHandler; //The GunHandler script on the gun object

    public Bullet thisBullet; //The current ScriptableObject Bullet in use

    public SpriteRenderer spriteRenderer; //This object's SpriteRenderer

    void Start()
    {
        gun = GameObject.FindWithTag("Gun");
        gunHandler = gun.GetComponent<GunHandler>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = thisBullet._bulletSprite;
        thisBullet = gunHandler.currentBullet;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            var AI = col.gameObject;
            GeneralAI genAI = AI.GetComponent<GeneralAI>();
            genAI.TakeDamage(gunHandler.damage, gunHandler.armorPen);
        }
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 5f);
        Destroy(gameObject);
    }

    void Awake()
    {
        StartCoroutine(BulletLifetime());
    }

    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
