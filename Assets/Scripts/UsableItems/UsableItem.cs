using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public int Id;
    public string itemName;

    [SerializeField] private GameObject itemScriptObject;
    
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
