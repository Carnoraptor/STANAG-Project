using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Create New Gun", order = 1)]
public class Gun : ScriptableObject
{
    [Header("Main")]
    public string _gunName;
    public enum GunType
    {
        Pistol,
        AssaultRifle,
        MachineGun,
        SubmachineGun,
        Shotgun,
        DMR,
        Explosive,
        Unique
    }
    public GunType _gunType;
    public int gunID;

    [Header("Stats")]
    public int _damage;
    public float _fireRate;
    public float _inaccuracy;
    public int _armorPen;
    public float _bulletSpeed;
    public float _radius;
    public int _bulletsAtOnce;
    public Bullet _bullet;
    public enum FireMode
    {
        SemiAutomatic,
        Automatic,
        Burst,
        Unique
    }
    public FireMode _fireMode;

    [Header("Graphics and Prefabs")]
    public GameObject _bulletPrefab;
    public Sprite _gunSprite;
    public Vector2 _bulletOriginOffset;
    //public Anim gunShooting

    [Header("Attachments")]
    public bool _hasMuzzleSlot;
    public bool _hasOpticSlot;
    public bool _hasLowerRailSlot;
    public bool _hasSideRailSlot;
    //public bool _hasStockSlot;

    public Vector2 _muzzleLoc;
    public Vector2 _opticLoc;
    public Vector2 _lowerRailLoc;
    public Vector2 _sideRailLoc;
    //public Vector2 _stockLoc;

}
