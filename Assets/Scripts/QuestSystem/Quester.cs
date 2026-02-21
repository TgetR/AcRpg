using UnityEngine;

public class Quester : MonoBehaviour
{
    public QuestChecker questChecker;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.T))
        {
            questChecker.TakeQuest("Kill 5 enemies", 1, 5);
        }
    }
}