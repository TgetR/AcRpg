using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<GameObject> slots = new List<GameObject>();
    private int activeSlots = 0;
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public GameObject InventoryUi;
    private bool opened = false;
    void Start()
    {
        GetSlots();
        List<ShopItemData> shopItems = Resources.LoadAll<ShopItemData>("Shop/Items").ToList();
        GetAndConvertShopItems(shopItems);
    }
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.I) )
        {
            if (opened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
    void GetSlots()
    {
        slots = GameObject.FindGameObjectsWithTag("InventorySlot").ToList();
    }

    void GetAndConvertShopItems(List<ShopItemData> shopItems)
    {
        foreach (ShopItemData shopItem in shopItems)
        {
            if (shopItem.purchased)
            {
                InventoryItem newInventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
                newInventoryItem.isConvertedFromShopItem = true;
                newInventoryItem.itemIdInShop = shopItem.itemId;
                newInventoryItem.itemId = shopItem.itemId;
                newInventoryItem.icon = shopItem.icon;
                newInventoryItem.itemDescription = shopItem.itemDescription;

                inventoryItems.Add(newInventoryItem);
                slots[activeSlots].transform.GetChild(0).GetComponent<Image>().sprite = shopItem.icon;
                slots[activeSlots].GetComponentInChildren<TMP_Text>().text = shopItem.itemDescription + "\n" + shopItem.effectDescription;
                activeSlots++;
            }
        }
    }

    public void OpenInventory()
    {
        InventoryUi.SetActive(true);
        GetAndConvertShopItems(Resources.LoadAll<ShopItemData>("Shop/Items").ToList());
        opened = true;
    }
    public void CloseInventory()
    {
        InventoryUi.SetActive(false);
        opened = false;
    }
}
