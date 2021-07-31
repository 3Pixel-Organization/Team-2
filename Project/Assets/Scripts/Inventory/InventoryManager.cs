using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    //this script is in ItemManager object inside GameManager
    //this script handle all inventory data for the player

    public static InventoryManager Instance;

    public Dictionary<string, Item_SO> items = new Dictionary<string, Item_SO>();
    public List<string> currentItems{
        get{return SaveManager.Instance.playerData.inventoryItems;}
        set{SaveManager.Instance.playerData.inventoryItems = value;}
    }
    public List<string> tempItems = new List<string>();


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[InventoryManager] There is more then one inventory Instance");
            return;
        }
        Instance = this;

        LoadResources();
    }

    void Start(){
        SyncData();
    }

    /// <summary>
    /// Make sure the data between player data and scriptable object match
    /// </summary>
    void SyncData(){
        //remove item from player data if no match id from item list
        for (int i = currentItems.Count-1; i >= 0; i--)
        {
            if(items.ContainsKey(currentItems[i]) == false){
                currentItems.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Load All items in Resources foldes
    /// </summary>
    void LoadResources(){
        items = new Dictionary<string, Item_SO>();
        List<Item_SO> tempItems = Resources.LoadAll<Item_SO>("Items").ToList();

        foreach (var item in tempItems)
        {
            items.Add(item.id, item);
        }
    }

    /// <summary>
    /// get all owned items based on id from saved data
    /// </summary>
    public List<Item_SO> OwnedItems(){
        List<Item_SO> temps = new List<Item_SO>();
        foreach (var id in currentItems)
        {
            temps.Add(items[id]);
        }
        return temps;
    }

    /// <summary>
    /// get all item for the store
    /// </summary>
    public List<Item_SO> StoreItems(){
        List<Item_SO> temps = new List<Item_SO>();
        foreach (var item in items)
        {
            if(item.Value.showInStore){
                temps.Add(item.Value);
            }
        }
        return temps;
    }

    /// <summary>
    /// Add an item to the inventory
    /// </summary>
    public void AddItemToInventory(Item_SO _item){
        if(_item.isStackable){
            for (int i = 0; i < currentItems.Count; i++)
            {
                if(currentItems[i] == _item.id){
                    items[currentItems[i]].stackSize += _item.stackSize;
                    return;
                }
            }
        }
        currentItems.Add(_item.id);
    }

    /// <summary>
    /// remove an item from the inventory
    /// </summary>
    public void RemoveItemFromInventory(Item_SO _item){
        if(!currentItems.Contains(_item.id)) return;
        if(_item.isStackable){
            for (int i = 0; i < currentItems.Count; i++)
            {
                if(currentItems[i] == _item.id){
                    items[currentItems[i]].stackSize -= _item.stackSize;
                    return;
                }
            }
        }
        currentItems.Remove(_item.id);
    }

    /// <summary>
    /// Get all items in temporary inventory and move them to inventory
    /// </summary>
    public void TransferTempToInventory(){
        for (int i = 0; i < tempItems.Count; i++)
        {
            AddItemToInventory(items[tempItems[i]]);
        }
        tempItems.Clear();
    }

    /// <summary>
    /// Add an item to the temporary inventory
    /// </summary>
    public void AddItemToTemp(Item_SO _item){
        tempItems.Add(_item.id);
    }
}