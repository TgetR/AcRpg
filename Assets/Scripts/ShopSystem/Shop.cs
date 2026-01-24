using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    List<GameObject> ItemButtons = new List<GameObject>(); // Лист карточек предметов в магазине

    private StatsManager _statsManager;
    private OnScreenNotify _notifyer;
    private GameObject _mainMenu;
    public List<ShopItemData> availableItems;

    void Start()
    {
        _mainMenu = transform.GetChild(1).gameObject;
        _statsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _notifyer = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
        availableItems = Resources.LoadAll<ShopItemData>("Shop/Items").ToList();
        
        Debug.Log("Items in object list:");
        foreach (var item in availableItems)
        {
            Debug.Log($"{item.itemDescription} with ID {item.itemId}");
        }
        
        
        //Заполение карточек товарами
        _mainMenu.SetActive(true);
        ItemButtons = GameObject.FindGameObjectsWithTag("ItemCard").ToList();
        ShuffleAndShowItems();
        Debug.Log("Shop initialized with " + availableItems.Count + " items and " + ItemButtons.Count + " item cards.");
        _mainMenu.SetActive(false);
    }

    #region ShopMethods
    public void ShuffleAndShowItems()
    {
        // Перемешивание предметов
        for(int i = availableItems.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            var temp = availableItems[i];
            availableItems[i] = availableItems[rnd];
            availableItems[rnd] = temp;
        }

        for(int i = 0; i < ItemButtons.Count(); i++ )
        {
            GameObject card = ItemButtons[i];
            
            var item = availableItems[i];
            if (item.purchased)
            {
                foreach(var obj in availableItems)
                {
                    if(!obj.purchased)
                    {
                        item = obj;
                        break;
                    }
                }
            }
            FillButton(card, item, i);
            Debug.Log("Item card " + i + " filled.");
        }
    }   

    private void FillButton(GameObject Button, ShopItemData item, int i)
    {
            Button.transform.GetChild(0).GetComponent<IdHolder>().ItemId = item.itemId;
            Button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.icon;

            Button.transform.GetChild(1).GetComponent<TMP_Text>().text = item.itemDescription;
            Button.transform.GetChild(2).GetComponent<TMP_Text>().text = item.effectDescription;
            Button.transform.GetChild(3).GetComponent<TMP_Text>().text = item.cost.ToString() + " Gold";
            Button.transform.GetChild(4).GetComponent<IdHolder>().ItemId = item.itemId;
            Debug.Log("Item card " + i + " set to " + item.itemDescription);
    }

    private void ActivateItem(int id)
    {
        ShopItemData item = availableItems.FirstOrDefault(obj => obj.itemId == id);
        
        if (item == null)
        {
            Debug.LogWarning($"Item with ID {id} not found!");
            return;
        }
        
        _statsManager.AddStatBonus(StatsManager.StatType.Health, item.buffHealth - item.debuffHealth);
        _statsManager.AddStatBonus(StatsManager.StatType.Speed, item.buffSpeed - item.debuffSpeed);
        _statsManager.AddStatBonus(StatsManager.StatType.Damage, item.buffDamage - item.debuffDamage);
        _statsManager.AddXpMultiplier(item.buffXpMultiplier - item.debuffXpMultiplier);
        
        Debug.Log($"Item {id} ({item.itemDescription}) activated!");
    }
    #endregion

    #region UI Methods
    public void BuyShuffle()
    {
        if(_statsManager.Gold >= 50)
        {
          ShuffleAndShowItems();  
          _statsManager.Gold -= 50;
        }
        else
        {
            Debug.Log("Not enough gold to shuffle shop items.");
            _notifyer.Notify("Not enough gold to shuffle!", 4);
        }
        
        Debug.Log("Shop items shuffled and displayed.");
    }

    public void BuyItem(Button sender)
    {
        int id = sender.transform.GetComponent<IdHolder>().ItemId;
        Debug.Log("Attempting to buy item with ID: " + id);
        foreach (var item in availableItems)
        {
           if(item.itemId == id)
           {
                Debug.Log("Cost: " + item.cost + ", Gold: " + _statsManager.Gold);
                if(_statsManager.Gold >= item.cost)
                {
                    _statsManager.Gold -= item.cost;
                    ActivateItem(id);
                    item.purchased = true;
                    Debug.Log("Item " + id + "(ID) purchased successfully.");
                    break;
                }
                else
                {
                    Debug.Log("Not enough gold to purchase item " + id + "(ID).");
                    _notifyer.Notify("Not enough gold to buy this item!", 4);
                    return;
                }
           }
        }

        // Refresh item display after purchase
        GameObject parent  = sender.transform.parent.gameObject;
        
        sender.GetComponent<IdHolder>().ItemId = -1;
        sender.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Shop/EmptySlotImage");

        parent.transform.GetChild(1).GetComponent<TMP_Text>().text = "Sold Out";
        parent.transform.GetChild(2).GetComponent<TMP_Text>().text = "This item has been purchased.";
        parent.transform.GetChild(3).GetComponent<TMP_Text>().text = "";
    }

    public void ExitShop()
    {
        Debug.Log("Exiting shop.");
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    }
    #endregion