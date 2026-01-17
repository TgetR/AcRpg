using UnityEngine;

public class ReturnToPreviosLocation : MonoBehaviour
{
    public bool isWorking = true;
    [SerializeField] Vector2 tpTo;
    GameObject canvas;
    GameObject player;
    private OnScreenNotify _notify;
    private bool inZone = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && inZone && isWorking)
        {
            _notify.Notify("You returned to previos location!", 1);
            player.transform.position = tpTo;   
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
