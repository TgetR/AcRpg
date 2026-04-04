using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<GameObject> _slots = new List<GameObject>();
    private int _activeSlots = 0;
    private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
    private bool _opened = false;

    private TMP_Text _keysSlotDescription;
    [SerializeField] private GameObject keysSlot;
    

    public GameObject InventoryUi;
    
    void Start()
    {
        GetSlots();
        List<ShopItemData> shopItems = Resources.LoadAll<ShopItemData>("Shop/Items").ToList();
        GetAndConvertShopItems(shopItems);

        _keysSlotDescription = keysSlot.GetComponentInChildren<TMP_Text>();
        _keysSlotDescription.text = "Keys \n You have:" + 0;
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.I)  && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
        {
            if (_opened)
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
        _slots = GameObject.FindGameObjectsWithTag("InventorySlot").ToList();
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

                _inventoryItems.Add(newInventoryItem);
                _slots[_activeSlots].transform.GetChild(0).GetComponent<Image>().sprite = shopItem.icon;
                _slots[_activeSlots].GetComponentInChildren<TMP_Text>().text = shopItem.itemDescription + "\n" + shopItem.effectDescription;
                _activeSlots++;
            }
        }
    }

    public void OpenInventory()
    {
        InventoryUi.SetActive(true);
        GetAndConvertShopItems(Resources.LoadAll<ShopItemData>("Shop/Items").ToList());
        _opened = true;

        Time.timeScale = 0f; // Pause the game when inventory is open

        _keysSlotDescription.text = "Keys \n You have:" + gameObject.GetComponent<KeySystemController>().KeysBalance;
    }

    public void CloseInventory()
    {
        Time.timeScale = 1f; // Resume the game when inventory is closed

        InventoryUi.SetActive(false);
        _opened = false;
    }

    public List<string> GetItemsNames_ForConsoleUse()
    {
        List<string> names = new List<string>();
        foreach (InventoryItem item in _inventoryItems)
        {
            names.Add(item.itemDescription);
        }
        return names;
    }
}
