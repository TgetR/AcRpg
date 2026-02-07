using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColdForestEnter : MonoBehaviour
{
    [SerializeField] private GameObject volumeDefault;
    [SerializeField] private GameObject volumeForest;
    [SerializeField] private GameObject GlobalLight;
    public bool isAnimationGoing;
    public float AnimnationGlobalLightIntensity;
    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           gameObject.GetComponent<Animator>().SetTrigger("CF_Enter");
            yield return new WaitForSeconds(1f);

            volumeDefault.GetComponent<Volume>().enabled = false;
            volumeForest.GetComponent<Volume>().enabled = true;
            GlobalLight.GetComponent<Light2D>().intensity = 0.1f;

            Debug.Log("Entered Cold Forest");
        }
    }

    IEnumerator OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("CF_Exit");
            yield return new WaitForSeconds(1.05f);

            volumeDefault.GetComponent<Volume>().enabled = true;
            volumeForest.GetComponent<Volume>().enabled = false;
            GlobalLight.GetComponent<Light2D>().intensity = 0.8f;

            Debug.Log("Exited Cold Forest");
        }
    }

    void Update()
    {
        if(isAnimationGoing) GlobalLight.GetComponent<Light2D>().intensity = AnimnationGlobalLightIntensity;
    
    }
}
