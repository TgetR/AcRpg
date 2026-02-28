using System.Collections.Generic;
using UnityEngine;

public class Quester : MonoBehaviour
{
    public int questTypeId = 0;
    public QuestChecker questChecker;
    public List<string> typeNames = new List<string>();
    [SerializeField] private GameObject questCanvas;
    void Start()
    {
        questCanvas.SetActive(false);
        typeNames.Add("Enemy Kill Quest"); // type id 0
        typeNames.Add("Collect Item Quest"); // type id 1
        typeNames.Add("Explore Location Quest"); // type id 2
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.T))
        {
            switch (questTypeId)
            {
                case 0:
                    //Enemy kill quest
                     questChecker.TakeQuest("Kill 5 enemies", 1, 5);
                    break;
                case 1:
                    //Collect item quest
                    break;
                case 2:
                    //Explore location quest
                    break;
                default:
                    Debug.Log("Unknown quest type.");
                    break;
            }
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