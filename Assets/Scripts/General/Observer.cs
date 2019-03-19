using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public abstract class IObserver
{
    private List<ISubject> registeredSubject = new List<ISubject>();

    public void RegisterSubject(ISubject subject)
    {
        registeredSubject.Add(subject);
    }

    public void RegisterObserver(IObserver observer)
    {
        for (int i = 0; i < registeredSubject.Count; i++)
        {
            if (registeredSubject[i].CheckObserver(observer))
            {
                continue;
            }
            else
            {
                registeredSubject[i].SubscribeObserver(observer);
            }
        }
    }

    public void DeregisterObserver(IObserver observer)
    {
        for (int i = 0; i < registeredSubject.Count; i++)
        {
            if (registeredSubject[i].CheckObserver(observer))
            {
                continue;
            }
            else
            {
                registeredSubject[i].UnsubscribeObserver(observer);
            }
        }
    }

    public abstract void Notify(NOTIFY_TYPE type);
}

public abstract class ISubject
{
    List<IObserver> observerList = new List<IObserver>();

    public void SubscribeObserver(IObserver obj)
    {
        observerList.Add(obj);
    }

    public void UnsubscribeObserver(IObserver obj)
    {
        for (int i = 0; i < observerList.Count; i++)
        {
            if (observerList[i] == obj)
            {
                observerList.RemoveAt(i);
                break;
            }
        }
    }

    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < observerList.Count; i++)
        {
            observerList[i].Notify(type);
        }
    }

    public bool CheckObserver(IObserver obj)
    {
        for (int i = 0; i < observerList.Count; i++)
        {
            if (observerList[i] == obj)
            {
                return true;
            }
        }

        return false;
    }
}*/

public interface IObserver
{
    void OnNotify(NOTIFY_TYPE type);
}

public interface ISubject
{
    void Notify(NOTIFY_TYPE type);
}

public enum NOTIFY_TYPE
{
    UI_HEALTH_BAR,
    UI_SHOOT_BUTTON,
    UI_INTERACT_BUTTON,

    ENTITY_IDLE,
    ENTITY_MOVE,
};