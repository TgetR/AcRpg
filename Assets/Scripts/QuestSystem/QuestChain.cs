using System.Collections;
using UnityEngine;

public class QuestChain : MonoBehaviour
{
    [SerializeField] private QuestChecker questChecker;

    int Type0Activations = 0; //Type 0 - Enemy kill quest
    int Type1Activations = 0; //Type 1 - Collect item quest
    int Type2Activations = 0; //Type 2 - Explore location quest
    //TYPE 1 AND TYPE 2 temporary not used 

    bool questTaken = false;
    bool questCompleted = false;
    bool delayEnd = true;
    private OnScreenNotify _notify;

    void Start()
    {
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }

    public void Type0ChainCheck()
    {
        if (delayEnd)
        {
            switch (Type0Activations)
            {
                case 0:
                    if (!questCompleted && !questTaken)
                    {
                    questChecker.TakeQuest("Kill 5 enemies", 1, 5, 50, 50);
                    Debug.Log("Type 0 quest taken.");
                    StartCoroutine(Delay());
                    }

                    else if (questCompleted && questTaken)
                    {
                        Type0Activations++;
                        questChecker.DeleteQuest();
                        Debug.Log("Type 0 quest completed and deleted.");
                        StartCoroutine(Delay());
                    }
                    return;
                case 1:
                    if (!questCompleted && !questTaken)
                    {
                    questChecker.TakeQuest("Kill 10 enemies", 1, 10, 150, 150);
                    Debug.Log("Type 0 quest taken. Stage 2");
                    StartCoroutine(Delay());
                    }

                    else if (questCompleted && questTaken)
                    {
                        Type0Activations++;
                        questChecker.DeleteQuest();
                        Debug.Log("Type 0 quest completed and deleted.");
                        StartCoroutine(Delay());
                    }
                    return;
                case 2:
                    if (!questCompleted && !questTaken)
                    {
                    questChecker.TakeQuest("Kill 20 enemies", 1, 20, 300, 300);
                    Debug.Log("Type 0 quest taken.");
                    StartCoroutine(Delay());
                    }

                    else if (questCompleted && questTaken)
                    {
                        Type0Activations++;
                        questChecker.DeleteQuest();
                        Debug.Log("Type 0 quest completed and deleted.");
                        StartCoroutine(Delay());
                    }
                    return;
                case 3:
                    _notify.Notify("No more quest for you! Find new adventures!", 2);
                    StartCoroutine(Delay()); break;
                default:
                    _notify.Notify("ERROR: Invalid quest stage. Try find another quest!", 4);
                    StartCoroutine(Delay()); break;

            }   
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) Debug.Log(Type0Activations);
        questTaken = questChecker.GetQuestTakenStatus();
        questCompleted = questChecker.GetQuestCompletedStatus();
    }

    IEnumerator Delay()
    {
        delayEnd = false;
        yield return new WaitForSeconds(1f);
        delayEnd = true;
    }
}
