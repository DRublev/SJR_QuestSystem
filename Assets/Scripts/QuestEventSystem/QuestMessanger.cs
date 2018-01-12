using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestMessanger : MonoBehaviour
{
    // Singletone realization
    private static QuestMessanger instance;
    public static QuestMessanger Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject("QuestMessanger", typeof(QuestMessanger));
                instance = gameObject.GetComponent<QuestMessanger>();
            }

            return instance;
        }
    }

    public class MessageHandler : Attribute { }

    private class MessageHandlerData
    {
        public object Container;
        public MethodInfo Method;
    }

    // Contains info about subscribers
    private Hashtable handlerHashtable = new Hashtable();

    private List<MessageHandlerData> removedList = new List<MessageHandlerData>();

    // Returns method name without "On"
    private string GetMethodName(string methodName)
    {
        return methodName.Substring(2); 
    }

    // Add event subscriber to hash
    public void RegisterMessageHandler(object container, MethodInfo methodInfo)
    {
        string messageID = GetMethodName(methodInfo.Name); 

        if(!handlerHashtable.ContainsKey(messageID))
        {
            handlerHashtable.Add(messageID, new List<MessageHandlerData>() { new MessageHandlerData() { Container = container, Method = methodInfo } });
        }

        // All message subscribers
        List<MessageHandlerData> messageHandlers = (List<MessageHandlerData>)handlerHashtable[messageID];

        // Add new message subscriber
        messageHandlers.Add(new MessageHandlerData() { Container = container, Method = methodInfo });
    }

    // Delete event subscriber from hash
    public void UnregisterMessageHandler(object container, MethodInfo methodInfo)
    {
        string messageID = GetMethodName(methodInfo.Name);

        if(handlerHashtable.ContainsKey(messageID))
        {
            List<MessageHandlerData> messageHandlers = (List<MessageHandlerData>)handlerHashtable[messageID];

            for(int i = 0; i < messageHandlers.Count; i++)
            {
                var messageHandler = messageHandlers[i];

                if(messageHandler.Container == container && messageHandler.Method == methodInfo)
                {
                    messageHandlers.Remove(messageHandler);
                    return;
                }
            }
        }
    }

    public void Call(string messageID, object[] parameter = null)
    {
        if(handlerHashtable.ContainsKey(messageID))
        {
            List<MessageHandlerData> handlerList = (List<MessageHandlerData>)handlerHashtable[messageID];

            for (int i = 0; i < handlerList.Count - 1; i++)
            {
                var messageHandler = handlerList[i];
                var unityObject = (MonoBehaviour)messageHandler.Container;

                if (unityObject != null)
                {
                    if(unityObject.gameObject.activeSelf)
                    {
                        messageHandler.Method.Invoke(messageHandler.Container, parameter);
                    }
                }
                else
                {
                    removedList.Add(messageHandler);
                }

            }

            for(int i = 0; i < removedList.Count; i++)
            {
                handlerList.Remove(removedList[i]);
            }

            removedList.Clear();
        }
    }

    // Clear handlers info on scene destroy
    void OnDestroy()
    {
        foreach(var handlers in handlerHashtable.Values)
        {
            List<MessageHandlerData> messageHandlers = (List<MessageHandlerData>)handlers;

            messageHandlers.Clear();
        }

        handlerHashtable.Clear();
        handlerHashtable = null;
    }
}