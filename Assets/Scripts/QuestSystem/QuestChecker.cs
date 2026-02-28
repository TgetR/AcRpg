using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    private StatsManager _Smanager;

    void Start()
    {
        questText.gameObject.SetActive(false);
        questProgressText.gameObject.SetActive(false);
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
    }

    void Update()
    {
        if(isEnemyKillQuestActive && !questCompleted) CheckEnemyKillQuest(enemyKillTarget);
    }

    public void TakeQuest(string questDescription, int typeId, float target)
    {
        if (!questTaked && !questCompleted)
        {
            questText.gameObject.SetActive(true);
            questText.text = questDescription;
            questProgressText.gameObject.SetActive(true);
            questProgressText.text = "Progress: 0/" + target;
            questTaked = true;
            enemyKillTarget = (int)target;
            Debug.Log("Quest taken: " + questDescription);
        }
        else if (questCompleted && questTaked)
        {
            _Smanager.Gold += 100; // Reward for completing the quest
            _Smanager.xpCount += 100; // Reward for completing the quest
            DeleteQuest();
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
        if (!questCompleted && questTaked)
        {
            questText.text = "Quest Completed! Take you reward.";
            questProgressText.gameObject.SetActive(false);
            questCompleted = true;
            Debug.Log("Quest completed!");
        }
    }

    public void DeleteQuest()
    {
        questText.gameObject.SetActive(false);
        questProgressText.gameObject.SetActive(false);
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
