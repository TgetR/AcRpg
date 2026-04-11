using TMPro;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public int Id;
    public string itemName;

    [SerializeField] private GameObject itemScriptObject;

    private Inventory _inventory;
    private int[] countOfItems = new int[3]; // Array to hold counts of each item type
    private bool isHotBarUpdating = true; // More for debug, to avoid calling HotBarUpdate()
    void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inventory>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)) //Random key for debug, not important
        {
            countOfItems[0] = 10;
            if(isHotBarUpdating) isHotBarUpdating = false;
            else isHotBarUpdating = true;
        }

        if (isHotBarUpdating) 
        {
            HotBarUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) && countOfItems[0] > 0)
        {
            Use();
            countOfItems[0]--;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) && countOfItems[1] > 0)
        {
            Use();
            countOfItems[1]--;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) && countOfItems[2] > 0)
        {
            Use();
            countOfItems[2]--;
        }
    }

    void HotBarUpdate()
    {
        GameObject hotbar = GameObject.FindGameObjectWithTag("HotBar");

        GameObject fisrtSlot = hotbar.transform.GetChild(0).gameObject; // Teleporter
        GameObject secondSlot = hotbar.transform.GetChild(1).gameObject; // Health Potion
        GameObject thirdSlot = hotbar.transform.GetChild(2).gameObject; // Empty Slot

        TMP_Text count_1st= fisrtSlot.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text count_2nd= secondSlot.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text count_3rd= thirdSlot.transform.GetChild(2).GetComponent<TMP_Text>();

        //Variables for count of each item in inventory
        int cnt1 = 0;
        int cnt2 = 0;
        int cnt3 = 0;
        _inventory.GetInventoryItems().ForEach(item =>
        {
            if (item.itemDescription == "Teleporter")
            {
                cnt1++;
            }
            else if (item.itemDescription == "Health Potion")
            {
                cnt2++;
            }
            else
            {
                cnt3++;
            }
        });

        count_1st.text = cnt1.ToString();
        count_2nd.text = cnt2.ToString();
        count_3rd.text = cnt3.ToString();
    }
    

    public void Use()
    {
        switch (itemName)
        {
            case "Teleporter":
                gameObject.GetComponent<Teleporter>().Teleport();
                Debug.Log("You used a Teleporter!");
                break;
            case "Health Potion":
                Debug.Log("You used a Health Potion!");
                break;
            default:
                Debug.Log($"You used {itemName}!");
                break;
        }
    }
}
