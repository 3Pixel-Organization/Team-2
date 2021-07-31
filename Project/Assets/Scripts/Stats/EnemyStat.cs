using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    public Enemy_SO enemyStat;

    public float aggroDistance;

    public override void InitCharacterStat()
    {
        if (enemyStat == null) Debug.LogError("enemystat is null");

        currentHealth = enemyStat.health;
        currentAttackPower = enemyStat.attack;
        critChance = enemyStat.criticalChance;
        critMultiplier = enemyStat.criticalMultiplier;

        attackRange = enemyStat.attackRange;
        attackInterval = enemyStat.attackInterval;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

    public override int GetMaxHealth()
    {
        return enemyStat.health;
    }

    public override void Dead()
    {
        base.Dead();
    }
}
