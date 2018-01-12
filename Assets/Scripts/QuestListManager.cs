using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListManager : QuestBehaviour
{
    private static QuestListManager instance;
    public static QuestListManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject questListManager = new GameObject("QuestListManager", typeof(QuestListManager));
                instance = questListManager.GetComponent<QuestListManager>();
            }

            return instance;
        }
    }

    public GameObject questListGameObject;
    private static Transform questListTransform;
    public RectTransform questItemPrefab; // Quest list item prefab
    private static RectTransform item;
    public int itemsOffset = 5;

    private static Vector2 delta;
    private static Vector3 elementPos;
    private static int size;
    private static float currentY;

    public static List<Quest> questsList = new List<Quest>();

    private void OnEnable()
    {
        questListTransform = questListGameObject.transform;
        item = questItemPrefab;

        delta = item.sizeDelta;
        delta.y += itemsOffset;
        elementPos = new Vector3(0, -delta.y / 2, 0);
    }
    
    public static void AddNewQuest(Quest quest)
    {
        item.gameObject.SetActive(true);
        currentY = -180;
        size++;

        foreach(RectTransform rect in questListTransform)
        {
            rect.anchoredPosition3D = new Vector3(elementPos.x, elementPos.y - currentY, 0);
            currentY += delta.y;
        }

        InstantiateTextItem(quest.Name);

        item.gameObject.SetActive(false);
        questsList.Add(quest);
    }

    private static void InstantiateTextItem(string name)
    {
        RectTransform itemClone = Instantiate(item) as RectTransform;
        itemClone.SetParent(questListTransform);
        itemClone.localScale = Vector3.one;
        itemClone.anchoredPosition3D = new Vector3(elementPos.x, elementPos.y - currentY, 0);
        itemClone.name = "Quest: " + name;

        Text itemText = itemClone.GetComponent<Text>();
        itemText.text = name;
    }

    private static void ClearQuestList()
    {
        for(int i = 0; i < questListTransform.childCount; i++)
        {
            var item = questListTransform.GetChild(i);
            Destroy(item.gameObject);
        }
    }

    private static void UpdateQuestList()
    {
        Debug.Log("Updating quest list");
        ClearQuestList();
        FillListWithQuests();
    }

    private static void FillListWithQuests()
    {
        foreach(Quest quest in questsList)
        {
            currentY = 0;
            AddNewQuest(quest);
        }
    }

    public static Quest GetFirstQuest()
    {
        Quest quest;

        if(questsList.Count > 0 && questsList[0] != null)
        {
            quest = questsList[0];
            return quest;
        }
        else
        {
            throw new Exception("Quest not foud!");
        }
    }
    
    public static void RemoveQuestFromList(Quest quest)
    {
        Debug.Log("Removing quest from list");
        questsList.Remove(quest);
        UpdateQuestList();
    }
}