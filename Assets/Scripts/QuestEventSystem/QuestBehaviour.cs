using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
 * Обертка для автоматической подписки / отписки от событий 
 */

public class QuestBehaviour : MonoBehaviour
{
    // Возможные модификатора метода
    private BindingFlags bindingFlags = 
        BindingFlags.Public |
        BindingFlags.Static |
        BindingFlags.NonPublic |
        BindingFlags.Instance |
        BindingFlags.FlattenHierarchy; 
	
    protected void Subscribe(string methodName)
    {
        MethodInfo method = this.GetType().GetMethod(methodName, bindingFlags);

        QuestMessanger.Instance.RegisterMessageHandler(this, method);
    }

    protected void Unsubscribe(string methodName)
    {
        MethodInfo method = this.GetType().GetMethod(methodName, bindingFlags);

        QuestMessanger.Instance.UnregisterMessageHandler(this, method);
    }

    // Auto subscribing on methods
    protected void Awake()
    {
        //Debug.Log("QuestBehaviour autosubscribe method");
        MethodInfo[] methods = this.GetType().GetMethods(bindingFlags);

        foreach (MethodInfo methodInfo in methods)
        {
            if (methodInfo.GetCustomAttributes(typeof(QuestMessanger.MessageHandler), true).Length != 0)
            {
                QuestMessanger.Instance.RegisterMessageHandler(this, methodInfo);
            }
        }
    }
}
