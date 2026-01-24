using System.Collections;
using UnityEngine;

public class HealHouse : MonoBehaviour
{
    public bool isWorking = true;
    private bool inZone = false;
    GameObject canvas;
    [SerializeField] GameObject menu;
    private bool _cooldown;
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;

        canvas.SetActive(false);
        menu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.E) && inZone && isWorking)
        {
                menu.SetActive(true);
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
