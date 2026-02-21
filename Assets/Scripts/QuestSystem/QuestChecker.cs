using System.Collections;
using TMPro;
using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    public TMP_Text questText;
    public TMP_Text questProgressText;
    private bool questTaked = false;
    private bool questCompleted = false;

    // Variables for enemy kill quest
    private bool isEnemyKillQuestActive = false;
    private int enemyKillCount = 0;
    private int enemyKillTarget = 0;

    void Start()
    {
        questText.gameObject.SetActive(false);
        questProgressText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckEnemyKillQuest(enemyKillTarget);
    }

    public void TakeQuest(string questDescription, int typeId, float target)
    {
        if (!questTaked)
        {
            questText.gameObject.SetActive(true);
            questText.text = questDescription;
            questProgressText.gameObject.SetActive(true);
            questProgressText.text = "Progress: 0/" + target;
            questTaked = true;
            enemyKillTarget = (int)target;
            Debug.Log("Quest taken: " + questDescription);
        }
        else
        {
            Debug.Log("You have already taken a quest.");
        }

        switch (typeId)
        {
            case 1:
                //First type - Enemy kill quest
                isEnemyKillQuestActive = true;
                 break;
            case 2:
                // Logic for quest type 2
                break;
            default:
                Debug.Log("Unknown quest type.");
                break;
        }
    }
    public void CompleteQuest()
    {
        if (!questCompleted)
        {
            questText.text = "Quest Completed! Take you reward.";
            questCompleted = true;
            Debug.Log("Quest completed!");
        }
        else
        {
            Debug.Log("You have already completed this quest.");
        }
    }

    public void DeleteQuest()
    {
        questText.gameObject.SetActive(false);
        questTaked = false;
        questCompleted = false;

        if(isEnemyKillQuestActive)
        {
        isEnemyKillQuestActive = false;
        enemyKillCount = 0;
        enemyKillTarget = 0;
        }

        //else if (other quest types) { reset their specific variables }

        Debug.Log("Quest deleted.");
    }

    void CheckEnemyKillQuest(float targetKills)
    {
        float currentKills = enemyKillCount;
        questProgressText.text = "Progress: " + currentKills + "/" + targetKills;
        if (currentKills >= targetKills) CompleteQuest();
    }

    public void AddEnemyKill()
    {
        if(isEnemyKillQuestActive) enemyKillCount++;
        //else ignore
    }
}
