using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    //this script is in ItemManager object inside GameManager
    //this script handle all equipement data for the player

    public static EquipmentManager Instance;

    public Item_SO defaultWeapon;

    public List<string> equipmentItems{
        get{return SaveManager.Instance.playerData.equipmentItems;}
        set{SaveManager.Instance.playerData.equipmentItems = value;}
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[EquipmentManager] There is more then one inventory Instance");
            return;
        }
        Instance = this;
    }

    void Start(){
        SyncData();
    }

    /// <summary>
    /// Make sure the data between player data and scriptable object match
    /// </summary>
    void SyncData(){
        //remove item from player data if no match id from item list
        for (int i = equipmentItems.Count-1; i >= 0; i--)
        {
            if(InventoryManager.Instance.items.ContainsKey(equipmentItems[i]) == false){
                equipmentItems.RemoveAt(i);
            }
        }
        if(equipmentItems.Count <= 0)
        {
            EquipItem(defaultWeapon);
        }
    }

    /// <summary>
    /// get all equipped items based on id in saved data
    /// </summary>
    public List<Item_SO> EquippedItems(){
        List<Item_SO> temps = new List<Item_SO>();
        foreach (var id in equipmentItems)
        {
            temps.Add(InventoryManager.Instance.items[id]);
        }
        return temps;
    }

    /// <summary>
    /// get item equipped in the slot
    /// </summary>
    public Item_SO GetEquipment(EquipmentType _type){
        for (int i = 0; i < equipmentItems.Count; i++)
        {
            Item_SO item = InventoryManager.Instance.items[equipmentItems[i]];
            if(item.equipmentType == _type){
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Equip Item
    /// </summary>
    public void EquipItem(Item_SO item)
    {
        equipmentItems.Add(item.id);
    }

    /// <summary>
    /// Unequip Item
    /// </summary>
    public void Unequip(Item_SO item)
    {
        equipmentItems.Remove(item.id);
    }

    /// <summary>
    /// get requipment stat total
    /// </summary>
    public float GetEquipmentStat(TypeOfAttributes typeOfAttributes)
    {
        float sum = 0;
        foreach (var item in equipmentItems)
        {
            sum += InventoryManager.Instance.items[item].GetItemAttribute(typeOfAttributes);
        }
        return sum;
    }
}