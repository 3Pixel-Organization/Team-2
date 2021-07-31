using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stat", menuName = "Stat/Enemy")]
public class Enemy_SO : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public int expGivenOnDeath;

    [Header("Attributes")]
    public int health;
    public int attack;
    public float speed;
    public float attackRange;
    public float attackInterval;
    [Range(0, 1)] public float criticalChance;
    [Range(0, 1)] public float criticalMultiplier;
}
