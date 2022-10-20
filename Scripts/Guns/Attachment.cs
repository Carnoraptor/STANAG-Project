using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Create New Attachment", order = 2)]
public class Attachment : ScriptableObject
{
    [Header("Main")]
    public string _attachmentName;
    public enum AttachmentType
    {
        muzzle,
        optic,
        lowerRail,
        sideRail
    }
    public AttachmentType _attachmentType;
    public int _attachmentID;

    [Header("Stats")]
    public int _damageMod;
    public float _fireRateMod;
    public float _accuracyMod;
    public int _armorPenMod;
    public float _bulletSpeedMod;
    public int _bulletsAtOnceMod;
    
    [Header("Graphics and Prefabs")]
    public bool _editsBullet;
    public GameObject _newBulletPrefab;
    public Sprite _attachmentSprite;
    //public Anim gunShooting

}
