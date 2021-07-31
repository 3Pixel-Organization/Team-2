using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    public Player_SO playerStat;

    public int currentShield;
    public int currentDefense;

    public int currentLevel;
    public int currentExp;
    public int currentCredit;
    public int currentAmmo;

    PlayerStatManager stat => PlayerStatManager.Instance;

    public override void InitCharacterStat()
    {
        if (playerStat == null) Debug.LogError("playerStat is null");

        currentHealth = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.HEALTH);
        currentShield = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.SHIELD);
        currentDefense = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.DEFENCE);
        currentAttackPower = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.ATTACK);
        critChance = stat.GetTotalAttributeLevel(TypeOfAttributes.CRITCHANCE);
        critMultiplier = stat.GetTotalAttributeLevel(TypeOfAttributes.CRITMULTIPLIER);

        currentLevel = SaveManager.Instance.playerData.playerLevel;
        currentExp = SaveManager.Instance.playerData.playerXP;
        currentAmmo = EquipmentManager.Instance.GetEquipment(EquipmentType.WEAPON).magazineSize;
        attackRange = EquipmentManager.Instance.GetEquipment(EquipmentType.WEAPON).attackRange;
        attackInterval = EquipmentManager.Instance.GetEquipment(EquipmentType.WEAPON).attackInterval;

        UIGameplayManager.Instance?.UpdatePlayerStatUI();
    }

    public override void TakeDamage(int amount)
    {
        //calculate damage after defense, defense take 1/3 of damage
        int def = Mathf.CeilToInt((float)amount / 3f);
        int defense = currentDefense;
        currentDefense -= def;
        if (currentDefense < 0) currentDefense = 0;
        amount -= Mathf.Abs(defense - currentDefense);

        //calculate damage after shield
        int shield = currentShield;
        currentShield -= amount;
        if (currentShield < 0) currentShield = 0;
        amount -= Mathf.Abs(shield - currentShield);

        if (amount < 0) amount = 0;
        base.TakeDamage(amount);
    }

    public override int GetMaxHealth()
    {
        return (int)stat.GetTotalAttributeLevel(TypeOfAttributes.HEALTH);
    }

    public override void Dead()
    {
        GameManager.Instance.DeathEventCall();
        base.Dead();
    }

    public void GiveHealth(int amount, out bool success)
    {
        int maxHealth = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.HEALTH);
        if (currentHealth >= maxHealth)
        {
            success = false;
            return;
        }
        success = true;
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void GiveShield(int amount, out bool success)
    {
        int maxShield = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.SHIELD);
        if (currentShield >= maxShield)
        {
            success = false;
            return;
        }
        success = true;
        currentShield += amount;
        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
        UIGameplayManager.Instance.UpdatePlayerStatUI();
    }

    public void GiveCredit(int amount)
    {
        currentCredit += amount;
    }

    public void LevelUpStatsChange()
    {
        currentHealth = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.HEALTH);
        currentShield = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.SHIELD);
        currentDefense = (int)stat.GetTotalAttributeLevel(TypeOfAttributes.DEFENCE);

        currentExp = 0;

        UIGameplayManager.Instance.UpdatePlayerStatUI();
    }

    public int MaxExp()
    {
        return stat.MaxCurrentLevelExp(currentLevel);
    }

    public void GiveExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= MaxExp())
        {
            while (currentExp >= MaxExp()) //level up
            {
                currentExp -= MaxExp();
                currentLevel++;
                LevelUpStatsChange();
            }
        }

        UIGameplayManager.Instance.UpdatePlayerStatUI();
    }

    public void AddAmmo(int size)
    {
        int maxAmmo = EquipmentManager.Instance.GetEquipment(EquipmentType.WEAPON).magazineSize;
        if (currentAmmo >= maxAmmo)
        {
            Debug.Log("Ammo is full");
            return;
        }

        currentAmmo += size;
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}
