using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColdForestEnter : MonoBehaviour
{
    [SerializeField] private GameObject volumeDefault;
    [SerializeField] private GameObject volumeForest;
    [SerializeField] private Light2D GlobalLight;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Animator>().SetTrigger("CF_Enter");
            volumeDefault.SetActive(false);
            volumeForest.SetActive(true);
            Debug.Log("a");
            GlobalLight.intensity = 0.1f;
        }
    }
}
