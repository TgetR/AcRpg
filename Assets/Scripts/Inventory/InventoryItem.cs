using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    [Header("ShopConversion")]
    public bool isConvertedFromShopItem = false;
    public int itemIdInShop;
    [Header("General")]
    public int itemId;
    public Sprite icon;
    public bool Usable = false;

    [Header("UI")]
    [TextArea] public string itemDescription;

}
