using UnityEngine;

public class ShopEntryConfirmMenu : MonoBehaviour
{
    [SerializeField] private GameObject ShopEntryCofirmObject;
    [SerializeField] private GameObject ShopMainMenuObject;
    public void EntryConfirmYes()
    {
        //close confirm menu
        ShopEntryCofirmObject.SetActive(false);
        //open shop menu
        ShopMainMenuObject.SetActive(true);
    }

    public void EntryConfirmNo()
    {
        //close confirm menu
        ShopEntryCofirmObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void EntryConfirmDebug()
    {
        
    }
}
