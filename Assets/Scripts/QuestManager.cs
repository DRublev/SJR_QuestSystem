using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : QuestBehaviour
{
    [QuestMessanger.MessageHandler]
    public void OnClickEvent()
    {
        Debug.Log("Success!");
    }

    // Calling to accepting process start
    [QuestMessanger.MessageHandler]
    public void OnAcceptQuestEvent(Quest quest)
    {
        Debug.Log("Quest accepted");
        QuestListManager.AddNewQuest(quest);
    }

    [QuestMessanger.MessageHandler]
    public void OnCompleteQuestEvent(Quest quest)
    {
        QuestListManager.RemoveQuestFromList(quest);
        Debug.Log("Quest completed");
    }
}
