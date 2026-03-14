using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    public string title;
    public int type;
    public int goal;
    public int xpReward;
    public int goldReward;
    public string[] takeDialogue;
    public string[] completeDialogue;
}