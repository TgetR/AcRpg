using NUnit.Framework;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EntryToArena : MonoBehaviour
{
    public bool isEntryCostKeys = true;
    public bool isWorking = true;
    public int KeysNeeded = 1;
    [SerializeField] private Vector2 Destination;
    [SerializeField] private KeySystemController _keySystemController;
    [SerializeField] private GameObject ConfirmMenu;
    [SerializeField] private ArenaSpawner ArenaSpawner;
    private OnScreenNotify _notify;
    GameObject canvas;
    private bool inZone = false;
    void Start()
    {
        ConfirmMenu.SetActive(false);
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();

        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.E) && inZone && isWorking)
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
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.GetComponent<PlayerController>().onArena && isWorking)
        {
            canvas.SetActive(true);
            inZone = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.GetComponent<PlayerController>().onArena && isWorking)
        {
            canvas.SetActive(false);
            inZone = false;
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