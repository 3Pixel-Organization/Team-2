using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBaseInventory : MonoBehaviour
{
    //this script is in InventoryUI in home scene 
    //this script handle all ui function for inventory ui in home scene

    public UIInventoryDesc inventoryDesc;

    [Header("Inventory")]
    public Transform inventoryParent;
    public GameObject inventoryPrefab;

    [Header("Equipment")]
    public UIEquipment[] equipmentSlots;

    [Header("Stats")]
    public List<AttributeUI> weaponAttributes = new List<AttributeUI>();

    public void InitInventory(){
        inventoryDesc.gameObject.SetActive(false);

        Inventory();
        Equipment();
        Stats();
    }

    void Inventory(){
        //clear parent
        for (int i = inventoryParent.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(inventoryParent.GetChild(i).gameObject);
        }

        foreach (var item in InventoryManager.Instance.currentItems)
        {
            UIItem uiItem = Instantiate(inventoryPrefab, inventoryParent).GetComponent<UIItem>();
            uiItem.InitInventoryItem(InventoryManager.Instance.items[item]);
        }
        for (int i = inventoryParent.childCount; i < 16; i++)
        {
            UIItem uiItem = Instantiate(inventoryPrefab, inventoryParent).GetComponent<UIItem>();
            uiItem.InitInventoryItem(null);
        }
    }

    void Equipment(){
        List<Item_SO> equipments = EquipmentManager.Instance.EquippedItems();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            Item_SO equipment = null;
            for (int j = 0; j < equipments.Count; j++)
            {
                if(equipmentSlots[i].equipmentType == equipments[j].equipmentType){
                    equipment = equipments[j];
                    break;
                }
            }
            equipmentSlots[i].InitEquipmentItem(equipment);
        }
    }

    void Stats(){
        foreach (var item in weaponAttributes)
        {
            item.InitEquipmentAttribute();
        }
    }

    /// <summary>
    /// Equip item from inventory to equipment
    /// </summary>
    public void EquipItem(Item_SO item){
        Item_SO equipped = EquipmentManager.Instance.GetEquipment(item.equipmentType);
        if(equipped != null){
            EquipmentManager.Instance.Unequip(equipped);
            InventoryManager.Instance.AddItemToInventory(equipped);
        }
        EquipmentManager.Instance.EquipItem(item);
        InventoryManager.Instance.RemoveItemFromInventory(item);

        InitInventory();
    }

    /// <summary>
    /// Unequip item from equipment to inventory
    /// </summary>
    public void UnequipItem(Item_SO item){
        EquipmentManager.Instance.Unequip(item);
        InventoryManager.Instance.AddItemToInventory(item);

        InitInventory();
    }
}
