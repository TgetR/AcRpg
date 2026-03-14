using TMPro;
using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    
    public bool GetQuestTakenStatus(){ return _questTaken; }
    public bool GetQuestCompletedStatus(){ return _questCompleted; }

    [SerializeField] private TMP_Text _questText;
    [SerializeField] private TMP_Text _questProgressText;

    private bool _questTaken = false; 
    private bool _questCompleted = false;

    // Variables for enemy kill quest
    private bool _isEnemyKillQuestActive = false;
    private int _enemyKillCount = 0;
    private int _enemyKillTarget = 0;
    // Variables for arena play quest
    private int _arenaEntryCount = 0;
    private bool _isArenaPlayQuestActive = false;
    private int _arenaPlayTarget = 0;
    private int _rewardGold;
    private int _rewardXP;


    private StatsManager _Smanager;

    //-----------------------------------------
    // Unity methods
    //-----------------------------------------
    void Start()
    {
        _questText.gameObject.SetActive(false);
        _questProgressText.gameObject.SetActive(false);
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
    }

    //-------------------------------
    // Quest management methods
    //-------------------------------
    public void QuestInteraction(string questDescription, int typeId, int target, int rewardGold = 100, int rewardXP = 100)
    {
        if (_questTaken && !_questCompleted) return; // Prevent taking a new quest if one is already taken and not completed
        else if (!_questTaken && !_questCompleted)
        {
            _rewardGold = rewardGold;
            _rewardXP = rewardXP;

            switch (typeId)
            {
                case 1:
                    //First type - Enemy kill quest
                    _isEnemyKillQuestActive = true;
                    _enemyKillTarget = target;
                    break;
                case 2:
                    // Second type - Arena playes quest
                    _isArenaPlayQuestActive = true;
                    _arenaPlayTarget = target;
                    break;
                default:
                    Debug.Log("Unknown quest type.");
                    break;
            }

            _questText.gameObject.SetActive(true);
            _questText.text = questDescription;
            _questProgressText.gameObject.SetActive(true);
            _questProgressText.text = "Progress: 0/" + target;
            _questTaken = true;
            Debug.Log("Quest taken: " + questDescription);
        }
        //Taking Reward for completed quest and deleting it
        else if (_questCompleted && _questTaken)
        {
            DeleteQuest();
        }
    }

    void TakeReward(int rewardGold, int rewardXP)
    {
        _Smanager.Gold += rewardGold; // Reward for completing the quest
        _Smanager.xpCount += rewardXP; // Reward for completing the quest
    }

    public void CompleteQuest()
    {
        if (!_questCompleted && _questTaken)
        {
            _questText.text = "Quest Completed! Take you reward.";
            _questProgressText.gameObject.SetActive(false);
            _questCompleted = true;
            Debug.Log("Quest completed!");
        }
    }

    public void DeleteQuest()
    {
        _questText.gameObject.SetActive(false);
        _questProgressText.gameObject.SetActive(false);
        _questTaken = false;
        _questCompleted = false;

        if(_isEnemyKillQuestActive)
        {
        _isEnemyKillQuestActive = false;
        _enemyKillCount = 0;
        _enemyKillTarget = 0;
        }
        else if (_isArenaPlayQuestActive)        
        {
            _isArenaPlayQuestActive = false;
            _arenaEntryCount = 0;
            _arenaPlayTarget = 0;
        }
        TakeReward(_rewardGold, _rewardXP);
        _rewardGold = 0;
        _rewardXP = 0;

        //else if (other quest types) { reset their specific variables }

        Debug.Log("Quest deleted.");
    }

    // --------------------------------
    // Specific quest type check methods
    // --------------------------------
    void CheckEnemyKillQuest(int targetKills)
    {
        int currentKills = _enemyKillCount;
        _questProgressText.text = "Progress: " + currentKills + "/" + targetKills;
        if (currentKills >= targetKills) CompleteQuest();
    }

    void CheckArenaPlayQuest(int targetEntries)
    {
        int currentEntries = _arenaEntryCount;
        _questProgressText.text = "Progress: " + currentEntries + "/" + targetEntries;
        if (currentEntries >= targetEntries) CompleteQuest();
    }

    //--------------------------------
    // Another specific quest type methods
    //--------------------------------
    public void AddEnemyKill()
    {
        if(_isEnemyKillQuestActive && !_questCompleted)
        {
          _enemyKillCount++;
          CheckEnemyKillQuest(_enemyKillTarget);
        } 
        //else ignore
    }
    public void AddArenaActivation()
    {
        if(_isArenaPlayQuestActive && !_questCompleted)
        {
          _arenaEntryCount++;
          CheckArenaPlayQuest(_arenaPlayTarget);
        } 
        //else ignore
    }
}
