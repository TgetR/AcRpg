using UnityEngine;

public class QuestChain : MonoBehaviour
{
    [SerializeField] private QuestChecker questChecker;

    int Type0Activations = -1; //Type 0 - Enemy kill quest
    int Type1Activations = 0; //Type 1 - Collect item quest
    int Type2Activations = 0; //Type 2 - Explore location quest
    //TYPE 1 AND TYPE 2 temporary not used 

    bool questTaken = false;
    bool questCompleted = false;
    public void Type0ChainCheck()
    {

        switch (Type0Activations)
        {
            case -1:
                if (!questCompleted && !questTaken)
                {
                questChecker.TakeQuest("Kill 5 enemies", 1, 5, 50, 50);
                Type0Activations++;
                Debug.Log("Type 0 quest taken.");
                }
                else if (questCompleted && questTaken)
                {
                    questChecker.DeleteQuest();
                    Debug.Log("Type 0 quest completed and deleted.");
                }
                return;
            case 0:
                if (!questCompleted && !questTaken)
                {
                questChecker.TakeQuest("Kill 10 enemies", 1, 10, 150, 150);
                Type0Activations++;
                Debug.Log("Type 0 quest taken. Stage 2");
                }
                else if (questCompleted && questTaken)
                {
                    questChecker.DeleteQuest();
                    Debug.Log("Type 0 quest completed and deleted.");
                }
                return;
            case 2:
                if (!questCompleted && !questTaken)
                {
                questChecker.TakeQuest("Kill 20 enemies", 1, 20, 300, 300);
                Type0Activations++;
                Debug.Log("Type 0 quest taken.");
                }
                else if (questCompleted && questTaken)
                {
                    questChecker.DeleteQuest();
                    Debug.Log("Type 0 quest completed and deleted.");
                }
                return;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) Debug.Log(Type0Activations);
        questTaken = questChecker.GetQuestTakenStatus();
        questCompleted = questChecker.GetQuestCompletedStatus();
    }
}
