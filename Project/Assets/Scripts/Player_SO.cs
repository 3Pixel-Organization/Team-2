using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stat", menuName = "Stat/Player")]
public class Player_SO : ScriptableObject
{
    public string playerName;
    public Sprite playerIcon;

    [Header("Level")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;
    public float expMaxlvlMultiplier = 1.25f; //how much the maxExp will increase on each level

    [Header("Default attributes")]
    public int health;
    public int shield;
    public int defense;
    public int attackPower;
    public float speed;
    [Range(0, 1)] public float criticalChance;
    [Range(0, 1)] public float criticalMultiplier;

    public float GetDefaultStat(TypeOfAttributes typeOfAttributes)
    {
        switch (typeOfAttributes)
        {
            case TypeOfAttributes.HEALTH:
                return health;
            case TypeOfAttributes.SHIELD:
                return shield;
            case TypeOfAttributes.ATTACK:
                return attackPower;
            case TypeOfAttributes.DEFENCE:
                return defense;
            case TypeOfAttributes.CRITCHANCE:
                return criticalChance;
            case TypeOfAttributes.CRITMULTIPLIER:
                return criticalMultiplier;
            case TypeOfAttributes.SPEED:
                return speed;
            default:
                return 0;
        }
    }
}
