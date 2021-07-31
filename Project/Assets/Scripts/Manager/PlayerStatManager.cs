using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;

    public Player_SO playerStat;

    List<AttributeUI_SO> attributes = new List<AttributeUI_SO>();

    PlayerData playerData => SaveManager.Instance.playerData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[PlayerStatManager] There is more then one Instance");
            return;
        }
        Instance = this;

        LoadResources();
    }

    /// <summary>
    /// Load All skills in Resources foldes
    /// </summary>
    void LoadResources()
    {
        attributes = Resources.LoadAll<AttributeUI_SO>("Attributes").ToList();
    }

    /// <summary>
    /// Get Attribute point from calculation
    /// </summary>
    public int GetAttributePoint()
    {
        int a = playerData.playerLevel;
        foreach (var item in playerData.playerAttributes)
        {
            a -= item.Value;
        }
        return a;
    }

    /// <summary>
    /// Get player attribute scriptable object
    /// </summary>
    public AttributeUI_SO GetAttribute(TypeOfAttributes typeOfAttributes)
    {
        foreach (var item in attributes)
        {
            if(item.attributeType == typeOfAttributes)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Get player attribute level from player data
    /// </summary>
    public float GetAttributeLevel(TypeOfAttributes typeOfAttributes, out bool maxed)
    {
        foreach (var item in playerData.playerAttributes)
        {
            if(item.Key == typeOfAttributes)
            {
                float value = playerStat.GetDefaultStat(typeOfAttributes) + (item.Value * GetAttribute(typeOfAttributes).addValue);
                maxed = (value >= GetAttribute(typeOfAttributes).maxValue);
                return value;
            }
        }
        maxed = false;
        return 0;
    }

    /// <summary>
    /// Get total player attribute
    /// </summary>
    public float GetTotalAttributeLevel(TypeOfAttributes typeOfAttributes)
    {
        foreach (var item in playerData.playerAttributes)
        {
            if (item.Key == typeOfAttributes)
            {
                float baseStat = GetAttributeLevel(item.Key, out bool maxed);
                float weaponStat = EquipmentManager.Instance.GetEquipmentStat(item.Key);
                return (baseStat + weaponStat);
            }
        }
        return 0;
    }

    /// <summary>
    /// Set player attribute level in player data
    /// </summary>
    public void AddAttributeLevel(TypeOfAttributes typeOfAttributes)
    {
        for (int i = 0; i < playerData.playerAttributes.Count; i++)
        {
            TypeOfAttributes key = playerData.playerAttributes.ElementAt(i).Key;
            if (key == typeOfAttributes)
            {
                playerData.playerAttributes[key]++;
                return;
            }
        }
    }

    /// <summary>
    /// get max exp need to upgrade from this level
    /// </summary>
    public int MaxCurrentLevelExp(int level = -1)
    {
        int max = playerStat.maxExp;
        if (level < 0) level = playerData.playerLevel;
        for (int i = 0; i < level; i++)
        {
            max = Mathf.RoundToInt(max * playerStat.expMaxlvlMultiplier);
        }
        return max;
    }
}

public enum TypeOfAttributes
{
    HEALTH,
    SHIELD,
    ATTACK,
    DEFENCE,
    CRITCHANCE,
    CRITMULTIPLIER,
    SPEED,
}
