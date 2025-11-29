using System.Collections;
using UnityEngine;

public class ElevationEntry : MonoBehaviour
{
    public Collider2D[] MountainColliders;
    private bool _onElevation = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Collider2D mountain in MountainColliders)
            {
                mountain.enabled = false;
            }

            if (!_onElevation)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
                _onElevation = true;
            }
            else
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _onElevation = false;
            }
        }
    }
}
