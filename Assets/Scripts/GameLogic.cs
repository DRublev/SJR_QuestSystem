using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameLogic
{
    public static void ClickEvent()
    {
        QuestMessanger.Instance.Call("ClickEvent");
    }

    public static void AcceptQuestEvent(Quest quest)
    {
        QuestMessanger.Instance.Call("AcceptQuestEvent", new object[] { quest });
    }

    public static void CompleteQuestEvent(Quest quest)
    {
        QuestMessanger.Instance.Call("CompleteQuestEvent", new object[] { quest });
    }
}

public partial class GameLogic : MonoBehaviour
{
    public void Click()
    {
        ClickEvent();
    }

    // Start point
    // Could be called in dialog with NPC
    public void AcceptQuest()
    {
        string questName = "Press X to win";
        int questExpReward = 300;
        Quest quest = new Quest(questName, questExpReward);

        AcceptQuestEvent(quest);
    }

    public void CompleteQuest()
    {
        Quest quest = QuestListManager.GetFirstQuest();

        if(QuestListManager.questsList.Exists(q => q.Equals(quest)))
        {
            Debug.Log("Quest exists. Trying to comlete it");
            CompleteQuestEvent(quest);
        }
    }
}
