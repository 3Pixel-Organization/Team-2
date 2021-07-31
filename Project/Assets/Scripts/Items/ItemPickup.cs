using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item_SO item;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            InventoryManager.Instance.AddItemToTemp(item);
            Destroy(gameObject);
        }
    }
}
