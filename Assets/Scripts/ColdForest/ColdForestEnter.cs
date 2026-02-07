using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColdForestEnter : MonoBehaviour
{
    [SerializeField] private EffectApplyer EffectsManager;

    [SerializeField] private GameObject volumeDefault;
    [SerializeField] private GameObject volumeForest;
    [SerializeField] private GameObject GlobalLight;
    public bool isAnimationGoing;
    public float AnimnationGlobalLightIntensity;

    private bool _isInForest = false;
    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           gameObject.GetComponent<Animator>().SetTrigger("CF_Enter");
            yield return new WaitForSeconds(1f);

            EffectsManager.ApplyEffect(1);
            Debug.Log("Entered Cold Forest");
            _isInForest = true;
            InvokeRepeating("InForestInvoke", 0f, 1f);
        }
    }

    IEnumerator OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("CF_Exit");
            yield return new WaitForSeconds(1.05f);

            Debug.Log("Exited Cold Forest");
            _isInForest = false;
        }
    }

    void InForestInvoke()
    {
        if (_isInForest)
        {
            EffectsManager.ApplyEffect(1);
        }
        else
        {
            CancelInvoke("InForestInvoke");
        }
    }
    public void ColdForestEnable()
    {
        volumeDefault.GetComponent<Volume>().enabled = false;
        volumeForest.GetComponent<Volume>().enabled = true;
        GlobalLight.GetComponent<Light2D>().intensity = 0.1f;
    }
    public void ColdForestDisable()
    {
        if (!_isInForest)
        {
            volumeDefault.GetComponent<Volume>().enabled = true;
            volumeForest.GetComponent<Volume>().enabled = false;
            GlobalLight.GetComponent<Light2D>().intensity = 0.8f;
        }

    }

    void Update()
    {
        if(isAnimationGoing) GlobalLight.GetComponent<Light2D>().intensity = AnimnationGlobalLightIntensity;
    
    }
}
