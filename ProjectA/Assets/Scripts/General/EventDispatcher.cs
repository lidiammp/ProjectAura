using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Event
{

}

public class PauseEvent : Event
{
    public float duration;
    public float timeScale;
}

public class ResumeEvent : Event
{
}
public class EventDispatcher : Singleton<EventDispatcher>
{
    public delegate void EventDelegate<T>(T e) where T : Event;

    private Dictionary<System.Type, System.Delegate> m_eventDelegates =
        new Dictionary<System.Type, System.Delegate>();
    public void AddListener<T>(EventDelegate<T> listener) where T: Event
    {
        System.Type type = typeof(T);
        System.Delegate del;

        if (m_eventDelegates.TryGetValue(type, out del))
        {
            del = System.Delegate.Combine(del, listener);
            m_eventDelegates[type] = del;
        }
        else
        {
            m_eventDelegates.Add(type, listener);
        }
    }

    public void RemoveListener<T>(EventDelegate<T> listener) where T : Event
    {
        System.Type type = typeof(T);
        System.Delegate del;

        if (m_eventDelegates.TryGetValue(type, out del))
        {
            System.Delegate newDel = System.Delegate.Remove(del, listener);

            if (newDel != null)
            {
                m_eventDelegates[type] = newDel;
            }
            else
            {
                m_eventDelegates.Remove(type);
            }
        }
    }
    public void SendEvent<T>(T evtData) where T: Event
    {
        System.Delegate del;

        if(m_eventDelegates.TryGetValue(typeof(T), out del))
        {
            EventDelegate<T> callback = del as EventDelegate<T>;
            if(callback != null)
            {
                callback(evtData);
            }
        }
    }
}