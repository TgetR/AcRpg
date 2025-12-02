using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Item")]
public class ShopItemData : ScriptableObject
{
    [Header("General")]
    public int itemId;
    public int cost;
    public Sprite icon;
    public bool purchased = false;

    [Header("Stats")]
    public int buffSpeed;
    public int buffHealth;
    public int buffDamage;
    public float buffXpMultiplier;

    public int debuffSpeed;
    public int debuffHealth;
    public int debuffDamage;
    public float debuffXpMultiplier;

    [Header("UI")]
    [TextArea] public string itemDescription;
    [TextArea] public string effectDescription;
}