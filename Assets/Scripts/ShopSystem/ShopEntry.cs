using System.Collections;
using UnityEngine;

public class ShopEntry : MonoBehaviour
{
    public GameObject ShopEntryCofirmObject;
    private bool _cooldown;

    void Start()
    {
        ShopEntryCofirmObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !_cooldown)
        {
            //Open ShopEntry confirm
            ShopEntryCofirmObject.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Entry");
            StartCoroutine(CooldownEntry());
        }
    }
    IEnumerator CooldownEntry()
    {
        _cooldown = true;
        yield return new WaitForSeconds(2);
        _cooldown = false;
    }
}
