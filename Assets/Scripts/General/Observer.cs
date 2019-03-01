using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void Notify();
}

public interface ISubject
{
    void SubscribeObserver(IObserver obj);

    void UnsubscribeObserver(IObserver obj);

    void Notify();

    bool CheckObserver(IObserver obj);
}

public enum CONTROLS
{

};