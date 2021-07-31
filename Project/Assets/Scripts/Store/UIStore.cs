using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIStore : MonoBehaviour
{
    public GameObject storeNodePrefab;
    public Transform sellParent;
    public Transform buyParent;

    [Header("Store Desc")]
    public GameObject descPanel;
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public Transform statParent;
    public GameObject statPrefab;
    public TextMeshProUGUI sellCostText;
    public TextMeshProUGUI buyCostText;
    public Button sellButton, buyButton;


    public void InitStore(){
        descPanel.SetActive(false);

        List<Item_SO> sellItems = InventoryManager.Instance.OwnedItems();
        InitList(sellItems, sellParent, out List<StoreItem> _sellItems);
        for (int i = 0; i < _sellItems.Count; i++)
        {
            _sellItems[i].InitSell(sellItems[i]);
        }

        List<Item_SO> buyItems = InventoryManager.Instance.StoreItems();
        InitList(InventoryManager.Instance.items.Values.ToList(), buyParent, out List<StoreItem> _buyItems);
        for (int i = 0; i < _buyItems.Count; i++)
        {
            _buyItems[i].InitBuy(buyItems[i]);
        }
    }

    void InitList(List<Item_SO> list, Transform _parent, out List<StoreItem> _storeItems){
        //clear parents
        for (int i = _parent.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(_parent.GetChild(i).gameObject);
        }
        List<StoreItem> storeItems = new List<StoreItem>();
        int c = list.Count>18?list.Count:18;
        for (int i = 0; i < c; i++)
        {
            StoreItem st = Instantiate(storeNodePrefab, _parent).GetComponent<StoreItem>();
            st.InitStoreItem();
            if(i < list.Count){
                storeItems.Add(st);
            }
        }
        _storeItems = storeItems;
    }

    //called in store items button
    public void OpenBuyDesc(Item_SO _item){
        SetDesc(_item);

        sellButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(true);
        buyCostText.text = _item.buyCost.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(()=> BuyItem(_item));
    }

    //called in store items button
    public void OpenSellDesc(Item_SO _item){
        SetDesc(_item);

        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(true);
        sellCostText.text = _item.sellCost.ToString();

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(()=> SellItem(_item));
    }

    /// <summary>
    /// set item data interface
    /// </summary>
    void SetDesc(Item_SO _item){
        descPanel.SetActive(true);

        titleText.text = _item.itemName;
        iconImage.sprite = _item.itemSprite;

        for (int i = statParent.childCount - 1; i >= 0; i--)
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
        descPanel.SetActive(false);
    }

    //button
    public void BuyItem(Item_SO _item){
        if(SaveManager.Instance.playerData.currentCredit >= _item.buyCost){
            Debug.Log("cek buy item " + _item.itemName);
            SaveManager.Instance.playerData.currentCredit -= _item.buyCost;
            InventoryManager.Instance.AddItemToInventory(_item);
            InitStore();
        }
    }

    //button
    public void SellItem(Item_SO _item){
        if(SaveManager.Instance.playerData.currentCredit >= _item.sellCost){
            Debug.Log("cek sell item " + _item.itemName);
            SaveManager.Instance.playerData.currentCredit -= _item.sellCost;
            InventoryManager.Instance.RemoveItemFromInventory(_item);
            InitStore();
        }
    }
}
