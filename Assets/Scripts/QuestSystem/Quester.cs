using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quester : MonoBehaviour
{
    [SerializeField] private QuestData questData;
    [SerializeField] private GameObject questCanvas;
    [SerializeField] private QuestChain questChain;
    void Start()
    {
        questCanvas.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.T)  && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
        {
            questChain.QuestChainCheck(questData, questData.type);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        questCanvas.SetActive(true);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        questCanvas.SetActive(false);
    }
}