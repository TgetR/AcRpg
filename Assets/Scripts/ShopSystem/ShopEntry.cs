using System.Collections;
using UnityEngine;

public class ShopEntry : MonoBehaviour
{
    public bool isWorking = true;
    private bool inZone = false;
    [SerializeField] private GameObject ShopMainMenuObject;
    GameObject canvas;

    void Start()
    {
        canvas = transform.GetChild(0).gameObject;

        canvas.SetActive(false);
        ShopMainMenuObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.E) && inZone && isWorking)
        {
                ShopMainMenuObject.SetActive(true);
                Time.timeScale = 0;
                canvas.SetActive(false);
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
