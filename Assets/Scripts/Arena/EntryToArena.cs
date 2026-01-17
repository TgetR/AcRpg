using NUnit.Framework;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EntryToArena : MonoBehaviour
{
    public bool isEntryCostKeys = true;
    public int KeysNeeded = 1;
    [SerializeField] private Vector2 Destination;
    [SerializeField] private KeySystemController _keySystemController;
    [SerializeField] private GameObject ConfirmMenu;
    [SerializeField] private ArenaSpawner ArenaSpawner;
    private OnScreenNotify _notify;
    void Start()
    {
        ConfirmMenu.SetActive(false);
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.GetComponent<PlayerController>().onArena)
        {
            if(isEntryCostKeys && KeysNeeded <= _keySystemController.KeysBalance || !isEntryCostKeys)
            {
                ConfirmMenu.SetActive(true);
                Time.timeScale = 0;  
            } 
            else
            {
                _notify.Notify("Not enough keys to enter!", 2);
            }
        }
    }

    public void ConfirmEntry(GameObject player)
    {
        _notify.Notify("You teleported to Arena!", 1);
        player.transform.position = Destination;
        player.GetComponent<PlayerController>().onArena = true;
        ConfirmMenu.SetActive(false);
        Time.timeScale = 1;
        ArenaSpawner.ActivateSpawner();
    }
    public void CancelEntry()
    {
        ConfirmMenu.SetActive(false);
        Time.timeScale = 1;
    }
}