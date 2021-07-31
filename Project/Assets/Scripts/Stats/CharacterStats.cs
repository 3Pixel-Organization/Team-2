using System;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public int currentHealth;
   
    public int currentAttackPower;
    public float critChance;
    public float critMultiplier;
    public float attackRange;
    public float attackInterval;

    public abstract void InitCharacterStat();

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead();
        }
        UIGameplayManager.Instance.UpdatePlayerStatUI();
    }

    public abstract int GetMaxHealth();

    public virtual void Dead()
    {
        Debug.Log("character dead", gameObject);
    }
}
