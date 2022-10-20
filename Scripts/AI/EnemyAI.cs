using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "AI/Create New AI/Hostile AI", order = 1)]
public class EnemyAI : ScriptableObject
{
    [Header("Main")]
    public string _enemyName;
    public enum EnemyType
    {
        None,
        Pursuer,
        Turret,
        Sniper,
        Charger,
        Custom
    }
    public EnemyType _enemyType;
    public int _enemyID;
    //public var enemyBehaviour;

    [Header("Stats")]
    public int _enemyHP;
    public int _enemyArmor;
    public int _enemyDamage;
    public int _enemyArmorPierce;
    public int _enemySpeed;
    public int _enemyAttackRate;

    [Header("Graphics and Prefabs")]
    public Sprite _enemySprite;
}
