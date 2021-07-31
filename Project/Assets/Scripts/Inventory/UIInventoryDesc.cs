using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryDesc : MonoBehaviour
{
    //this script is in popup panel for inventory items description in home scene 
    //this script handle all ui function for inventory ui description

    public TextMeshProUGUI titleText;
    public Image iconImage;
    public Transform statParent;
    public GameObject statPrefab;
    public Button equipButton, unequipButton;

    public void OpenUnequipDesc(Item_SO _item){
        SetDesc(_item);

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);

        unequipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.AddListener(()=> UnequipItem(_item));
    }

    public void OpenEquipDesc(Item_SO _item){
        SetDesc(_item);

        unequipButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(true);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(()=> EquipItem(_item));
    }

    /// <summary>
    /// set item data interface
    /// </summary>
    void SetDesc(Item_SO _item){
        gameObject.SetActive(true);

        titleText.text = _item.itemName;
        iconImage.sprite = _item.itemSprite;

        for (int i = statParent.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(statParent.GetChild(i).gameObject);
        }
        foreach (var item in _item.GetStats())
        {
            Instantiate(statPrefab, statParent).GetComponent<UIItemStat>().InitValue(item.Key, item.Value);
        }
    }

    //called in close button
    public void CloseDesc(){
        gameObject.SetActive(false);
    }

    //button
    public void UnequipItem(Item_SO _item){
        MainBaseManager.Instance.inventory.UnequipItem(_item);
    }

    //button
    public void EquipItem(Item_SO _item){
        MainBaseManager.Instance.inventory.EquipItem(_item);
    }
}
