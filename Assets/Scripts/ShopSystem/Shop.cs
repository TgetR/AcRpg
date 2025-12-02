using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    List<ShopItem> Objects = new List<ShopItem>(); // Лист предметов в магазине

    List<GameObject> ItemButtons = new List<GameObject>(); // Лист карточек предметов в магазине

    private StatsManager _statsManager;
    private OnScreenNotify _notifyer;
    private GameObject _mainMenu;

    void Start()
    {
        _mainMenu = transform.GetChild(1).gameObject;
        _statsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _notifyer = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();


        Type ourtype = typeof(ShopItem); // Базовый тип
        IEnumerable<Type> types = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));  // using System.Linq
        

        Debug.Log("Items in object list:");
        foreach(var obj in Objects)
            Debug.Log(obj.ItemDescription);
        
        foreach (Type myType in types)
        {
            ShopItem Obj = (ShopItem)Activator.CreateInstance(myType);
            Obj.purchased = false;
            Objects.Add(Obj);
        }
        
        
        //Заполение карточек товарами
        _mainMenu.SetActive(true);
        ItemButtons = GameObject.FindGameObjectsWithTag("ItemCard").ToList();
        ShuffleAndShowItems();
        Debug.Log("Shop initialized with " + Objects.Count + " items and " + ItemButtons.Count + " item cards.");
        _mainMenu.SetActive(false);
    }

    #region ShopMethods
    public void ShuffleAndShowItems()
    {
        // Перемешивание предметов
        for(int i = Objects.Count - 1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0, i + 1);
            var temp = Objects[i];
            Objects[i] = Objects[rnd];
            Objects[rnd] = temp;
        }

        for(int i = 0; i < ItemButtons.Count(); i++ )
        {
            GameObject card = ItemButtons[i];
            
            var item = Objects[i];
            if (item.purchased)
            {
                foreach(var obj in Objects)
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

    private void FillButton(GameObject Button, ShopItem item, int i)
    {
            Button.transform.GetChild(0).GetComponent<IdHolder>().ItemId = item.ItemId;
            Button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Shop/" + item.PathToImage);

            Button.transform.GetChild(1).GetComponent<TMP_Text>().text = item.ItemDescription;
            Button.transform.GetChild(2).GetComponent<TMP_Text>().text = item.EffectDescription;
            Button.transform.GetChild(3).GetComponent<TMP_Text>().text = item.cost.ToString() + " Gold";
            Button.transform.GetChild(4).GetComponent<IdHolder>().ItemId = item.ItemId;
            Debug.Log("Item card " + i + " set to " + item.ItemDescription);
    }

    private void ActivateItem(int id)
    {
        ShopItem item = Objects.FirstOrDefault(obj => obj.ItemId == id);
        
        if (item == null)
        {
            Debug.LogWarning($"Item with ID {id} not found!");
            return;
        }
        
        _statsManager.AddStatBonus(0, item.buffHealth - item.debuffHealth);
        _statsManager.AddStatBonus(1, item.buffSpeed - item.debuffSpeed);
        _statsManager.AddStatBonus(2, item.buffDamage - item.debuffDamage);
        _statsManager.AddXpMultiplier(item.buffXpMultiplier - item.debuffXpMultiplier);
        
        Debug.Log($"Item {id} ({item.ItemDescription}) activated!");
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
        foreach (var obj in Objects)
        {
           if(obj.ItemId == id)
           {
                if(_statsManager.Gold >= obj.cost)
                {
                    _statsManager.Gold -= obj.cost;
                    ActivateItem(id);
                    obj.purchased = true;
                    Debug.Log("Item " + id + "(ID) purchased successfully.");
                    break;
                }
                else
                {
                    Debug.Log("Not enough gold to purchase item " + id + "(ID).");
                    _notifyer.Notify("Not enough gold to buy this item!", 4);
                    break;
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
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    }
    #endregion


#region ShopItemClasses
class ShopItem
{
    //Common properties
    public virtual bool purchased { get; set; } = false;
    public virtual int ItemId { get; set; } = -1;
    public virtual int cost { get; set; } = -1;

    //Buffs
    public virtual int buffSpeed { get; set; } = 0;
    public virtual int buffHealth { get; set; } = 0;
    public virtual int buffDamage { get; set; } = 0;
    public virtual float buffXpMultiplier { get; set; } = 0;

    //Debuffs
    public virtual int debuffSpeed { get; set; } = 0;
    public virtual int debuffHealth { get; set; } = 0;
    public virtual int debuffDamage { get; set; } = 0;
    public virtual float debuffXpMultiplier { get; set; } = 0;

    //Descriptions and image path
    public virtual string ItemDescription { get; set; } = "Shop Item";
    public virtual string EffectDescription { get; set; } = "nothing";
    public virtual string PathToImage { get; set; } = "DefaultImage";
}

class MegaSword : ShopItem
{
    public override int ItemId { get; set; } = 0;
    public override int cost { get; set; } = 250;

    public override int buffDamage { get; set; }= 4;
    public override int debuffSpeed { get; set; } = 1;

    public override string ItemDescription { get; set; } = "Very big sword.";
    public override string EffectDescription { get; set; } = "+4 Damage; -1 Speed";
    public override string PathToImage { get; set; } = "MegaSwordImage";
}

class SpeedyBoots : ShopItem
{    
    public override int ItemId { get; set; } = 1;
    public override int cost { get; set; } = 100;

    public override int buffSpeed { get; set; } = 3;

    public override string ItemDescription { get; set; } = "Speedy boots.";
    public override string EffectDescription { get; set; } = "+3 Speed";
    public override string PathToImage { get; set; } = "SpeedyBootsImage";
}

class HeavyChestplate : ShopItem
{
    public override int ItemId { get; set; } = 2;
    public override int cost { get; set; } = 300;

    public override int buffHealth { get; set; } = 100;
    public override int debuffSpeed { get; set; } = 2;

    public override string ItemDescription { get; set; } = "Heavy chestplate";
    public override string EffectDescription { get; set; } = "+ 100 Health(Defense); -2 Speed";
    public override string PathToImage { get; set; } = "HeavyChestplateImage";
}
class XPAmulet : ShopItem
{
    public override int ItemId { get; set; } = 3;
    public override int cost { get; set; } = 200;

    public override float buffXpMultiplier { get; set; } = 0.5f;

    public override string ItemDescription { get; set; } = "Strange amulet";
    public override string EffectDescription { get; set; } = "+50% XP Gain";
    public override string PathToImage { get; set; } = "XPAmuletImage";
}
#endregion