using System.Collections;
using UnityEngine;

public class HealHouse : MonoBehaviour
{
    [SerializeField] GameObject menu;
    private bool _cooldown;
    void Start()
    {
        menu.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !_cooldown)
        {
            menu.SetActive(true);
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
