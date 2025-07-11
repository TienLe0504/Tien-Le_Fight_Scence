using System;
using System.Collections.Generic;
using UnityEngine;
public enum EventType
{
    Jump,
    Move,
    Attack,
    StartGame,
    TakeDamage,
    TurnOnUI,
    dataUI,
    winGame,
    loseGame,
    continueGame,
    HomeScreen,
    ReadyGo,
    StartGameWith25Models

}
public class ListenerManager : BaseManager<ListenerManager>
{

    private Dictionary<EventType, Delegate> eventTable = new Dictionary<EventType, Delegate>();

    public void AddListener(EventType eventType, Action callback)
    {
        if (eventTable.ContainsKey(eventType))
            eventTable[eventType] = (Action)eventTable[eventType] + callback;
        else
            eventTable[eventType] = callback;
    }

    public void AddListener<T>(EventType eventType, Action<T> callback)
    {
        if (eventTable.ContainsKey(eventType))
            eventTable[eventType] = (Action<T>)eventTable[eventType] + callback;
        else
            eventTable[eventType] = callback;
    }

    public void RemoveListener(EventType eventType, Action callback)
    {
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Action)eventTable[eventType] - callback;
            if (eventTable[eventType] == null)
                eventTable.Remove(eventType);
        }
    }

    public void RemoveListener<T>(EventType eventType, Action<T> callback)
    {
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Action<T>)eventTable[eventType] - callback;
            if (eventTable[eventType] == null)
                eventTable.Remove(eventType);
        }
    }
    public void TriggerEvent(EventType eventType)
    {
        if (eventTable.ContainsKey(eventType))
            (eventTable[eventType] as Action)?.Invoke();
    }

    public void TriggerEvent<T>(EventType eventType, T param)
    {
        if (eventTable.ContainsKey(eventType))
            (eventTable[eventType] as Action<T>)?.Invoke(param);
    }
}
