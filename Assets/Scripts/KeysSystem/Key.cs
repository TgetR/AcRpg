using UnityEngine;

public class Key : MonoBehaviour
{
    KeySystemController _keyDropController;
    private OnScreenNotify _notify;
    void Start()
    {
        _keyDropController = GameObject.FindGameObjectWithTag("KeyController").GetComponent<KeySystemController>();
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _keyDropController.KeysCanUse = true;
            _keyDropController.KeysBalance++;
            _notify.Notify("You reached a key!", 3);
            Destroy(this.gameObject);
        }
    }
}
