using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public PlayerData playerData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[SaveManager] There is more then one Instance");
            return;
        }
        Instance = this;
        Load();
    }

    /// <summary>
    /// Save Data
    /// </summary>
    [ContextMenu("Save Data")]
    public void Save()
    {
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerInfo.dat");

        PlayerData data = playerData;

        Debug.Log("save");

        bin.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// Load Data
    /// </summary>
    [ContextMenu("Load Data")]
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerInfo.dat"))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData)bin.Deserialize(file);
            file.Close();

            Debug.Log("Load");

            playerData = data;
        }else
        {
            playerData = new PlayerData();
        }
    }

    /// <summary>
    /// Delete Data
    /// </summary>
    [ContextMenu("Delete Data")]
    public void Delete()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerInfo.dat");
        }

        PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}

[System.Serializable]
public class PlayerData
{
    [Header("Player")]
    public string playerName;
    public int playerLevel;
    public int playerXP;
    public int currentCredit;

    [Header("Player Stats")]
    public Dictionary<TypeOfAttributes, int> playerAttributes = new Dictionary<TypeOfAttributes, int>();

    [Header("Inventory")]
    public List<string> inventoryItems = new List<string>();
    public List<string> equipmentItems = new List<string>();

    [Header("Skill")]
    public int skillPoint = 5;
    public SkillData meleeSkill = new SkillData();
    public SkillData gunSkill = new SkillData();
    public SkillData bodySkill = new SkillData();

    /// <summary>
    /// Make sure the data in player data is not null or the index is insufficient
    /// </summary>
    public void SyncPlayerData()
    {
        if (playerLevel <= 0) playerLevel = 1;

        //sync player stats
        List<TypeOfAttributes> attributes = ((TypeOfAttributes[])Enum.GetValues(typeof(TypeOfAttributes))).ToList();
        if (playerAttributes == null || playerAttributes.Count <= 0)
        {
            playerAttributes = new Dictionary<TypeOfAttributes, int>();
        }
        foreach (var item in attributes)
        {
            if(playerAttributes.ContainsKey(item) == false)
            {
                playerAttributes.Add(item, 0);
            }
        }
        foreach (var item in playerAttributes.Keys)
        {
            if(attributes.Contains(item) == false)
            {
                playerAttributes.Remove(item);
            }
        }

        //sync inventory
        if(inventoryItems == null || inventoryItems.Count <= 0)
        {
            inventoryItems = new List<string>();
        }
        if (equipmentItems == null || equipmentItems.Count <= 0)
        {
            equipmentItems = new List<string>();
        }
    }
}

[System.Serializable]
public class SkillData
{
    public int level = 0;
    public string equipped = string.Empty;
}
