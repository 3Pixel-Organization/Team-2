using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack.asset", menuName ="Attack/New Attack")]
public class AttackDefenition : ScriptableObject
{
    public int CalculateDamage (CharacterStats attacker)
    {
        int damage = attacker.currentAttackPower;

        bool isCritical = Random.value < attacker.critChance;
        if (isCritical)
        {
            damage += (int)(damage * attacker.critMultiplier);
        }

        return damage;
    }

    public void ExecuteAttack(GameObject attacker, GameObject target)
    {
        if (target == null)
            return;

        CharacterStats characterStats = attacker.GetComponent<CharacterStats>();

        // check if target is in range of player
        if (Vector3.Distance(attacker.transform.position, target.transform.position) > characterStats.attackRange)
            return;

        // check if target is in front of the player
        if (!attacker.transform.IsFacingTarget(target.transform))
            return;

        // at this point the attack will connect
        var attack = CalculateDamage(characterStats);

        var attackables = target.GetComponents<IAttackable>();

        foreach (var a in attackables)
        {
            ((IAttackable)a).OnAttack(attacker.gameObject, attack);
        }
    }
}
