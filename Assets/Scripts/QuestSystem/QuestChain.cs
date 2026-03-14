using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChain : MonoBehaviour
{
    [SerializeField] private QuestChecker questChecker;
    [SerializeField] private Dialogue dialogueManager;
    private OnScreenNotify _notify;

    private bool delayEnd = true;
    private List<int> QuestActivations = new List<int>(); // [ID] - Quest ID by type, number in [ID] - Activations of this quest type

    void Start()
    {
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }

    public void QuestChainCheck(QuestData questData, int typeID)
    {
        bool questTaken = questChecker.GetQuestTakenStatus();
        bool questCompleted = questChecker.GetQuestCompletedStatus();

        while (QuestActivations.Count <= typeID)
                QuestActivations.Add(0);
        
        int stage = QuestActivations[typeID];
        if (stage >= questData.takeDialogue.Length)
        {
            _notify.Notify("No more quests! Find new adventures!", 2);
            return;
        }

        if (delayEnd)
        {
            if(!questTaken && !questCompleted)
            {
                List<string> DialogueText = new List<string>(questData.takeDialogue[stage].Split('/'));
                dialogueManager.StartDialogue(DialogueText);
                questChecker.QuestInteraction(questData.title, questData.type, questData.goal, questData.xpReward * (stage + 1), questData.goldReward * (stage + 1));
                Debug.Log($"Type #{typeID}-{stage} quest taken.");
                StartCoroutine(Delay());
            }

            else if (questCompleted && questTaken)
            {
                QuestActivations[typeID]++;
                questChecker.DeleteQuest();
                List<string> DialogueText = new List<string>(questData.completeDialogue[stage].Split('/'));
                dialogueManager.StartDialogue(DialogueText);
                Debug.Log($"Type #{typeID}-{stage} quest completed and deleted.");
                StartCoroutine(Delay());
            }
            else if(!questCompleted && questTaken)
            {
                _notify.Notify("You haven't completed the quest yet! Complete it first!", 3);
                StartCoroutine(Delay());
            }
        }
    }

    IEnumerator Delay()
    {
        delayEnd = false;
        yield return new WaitForSeconds(1f);
        delayEnd = true;
    }
}
