using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if(Input.GetKey(KeyCode.E) && inZone && isWorking  && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
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
