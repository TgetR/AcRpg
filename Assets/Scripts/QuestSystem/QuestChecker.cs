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
    
    public bool GetQuestTakenStatus(){ return questTaked; }
    public bool GetQuestCompletedStatus(){ return questCompleted; }

    // Variables for enemy kill quest
    private bool isEnemyKillQuestActive = false;
    private int enemyKillCount = 0;
    private int enemyKillTarget = 0;
    // Variables for arena play quest
    [SerializeField] private EntryToArena arenaEntry;
    private bool isArenaPlayQuestActive = false;
    private int arenaPlayTarget = 0;


    private StatsManager _Smanager;

    //-----------------------------------------
    // Unity methods
    //-----------------------------------------
    void Start()
    {
        questText.gameObject.SetActive(false);
        questProgressText.gameObject.SetActive(false);
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
    }

    void Update()
    {
        if(isEnemyKillQuestActive && !questCompleted) CheckEnemyKillQuest(enemyKillTarget);
        else if (isArenaPlayQuestActive && !questCompleted) CheckArenaPlayQuest(arenaPlayTarget);
    }

    //-------------------------------
    // Quest management methods
    //-------------------------------
    public void TakeQuest(string questDescription, int typeId, float target, int rewardGold = 100, int rewardXP = 100)
    {
        if (!questTaked && !questCompleted)
        {
            questText.gameObject.SetActive(true);
            questText.text = questDescription;
            questProgressText.gameObject.SetActive(true);
            questProgressText.text = "Progress: 0/" + target;
            questTaked = true;
            Debug.Log("Quest taken: " + questDescription);
        }
        else if (questCompleted && questTaked)
        {
            _Smanager.Gold += rewardGold; // Reward for completing the quest
            _Smanager.xpCount += rewardXP; // Reward for completing the quest
            DeleteQuest();
        }
        switch (typeId)
        {
            case 1:
                //First type - Enemy kill quest
                isEnemyKillQuestActive = true;
                enemyKillTarget = (int)target;
                 break;
            case 2:
                // Second type - Arena playes quest
                isArenaPlayQuestActive = true;
                arenaPlayTarget = (int)target;
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
        else if (isArenaPlayQuestActive)        
        {
            isArenaPlayQuestActive = false;
            arenaEntry.EntryNumber = 0;
        }

        //else if (other quest types) { reset their specific variables }

        Debug.Log("Quest deleted.");
    }

    // --------------------------------
    // Specific quest type check methods
    // --------------------------------
    void CheckEnemyKillQuest(float targetKills)
    {
        float currentKills = enemyKillCount;
        questProgressText.text = "Progress: " + currentKills + "/" + targetKills;
        if (currentKills >= targetKills) CompleteQuest();
    }

    void CheckArenaPlayQuest(float targetEntries)
    {
        float currentEntries = arenaEntry.EntryNumber;
        questProgressText.text = "Progress: " + currentEntries + "/" + targetEntries;
        if (currentEntries >= targetEntries) CompleteQuest();
    }

    //--------------------------------
    // Another specific quest type methods
    //--------------------------------
    public void AddEnemyKill()
    {
        if(isEnemyKillQuestActive) enemyKillCount++;
        //else ignore
    }
}
