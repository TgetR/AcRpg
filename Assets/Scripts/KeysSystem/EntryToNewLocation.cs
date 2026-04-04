using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntryToNewLocation : MonoBehaviour
{
    public bool isWorking = true;
    [SerializeField] Vector2 tpTo;
    GameObject canvas;
    GameObject player;
    private OnScreenNotify _notify;
    private bool inZone = false;
    private KeySystemController _keySystemController;
    
    void Start()
    {
        _keySystemController = GameObject.FindGameObjectWithTag("GameController").GetComponent<KeySystemController>();
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.E) && inZone  && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
        {
            if (isWorking && _keySystemController.KeysCanUse && _keySystemController.KeysBalance > 0)
            {
                _notify.Notify("You enter to new location!", 1);
                player.transform.position = tpTo;   
            }
            else
            {
                _notify.Notify("You can't enter to this location!", 3);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            canvas.SetActive(true);
            inZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.transform.CompareTag("Player"))
        {
           canvas.SetActive(false); 
           inZone = false;
        } 
    }
}
