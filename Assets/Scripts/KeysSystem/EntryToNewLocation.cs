using UnityEngine;

public class EntryToNewLocation : MonoBehaviour
{
    public Vector2 tpTo;
    KeySystemController _keySystemController;
    private OnScreenNotify _notify;
    void Start()
    {
        _keySystemController = GameObject.FindGameObjectWithTag("KeyController").GetComponent<KeySystemController>();
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _notify.Notify("You teleported to new location!", 4);
            collision.transform.position = tpTo;
        }
    }
}
