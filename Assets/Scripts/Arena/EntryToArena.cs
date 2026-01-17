using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EntryToArena : MonoBehaviour
{
    [SerializeField] private Vector2 Destination;
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
            ConfirmMenu.SetActive(true);
            Time.timeScale = 0;  
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