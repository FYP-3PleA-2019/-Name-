using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager mInstance;

    public static UIManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("UIManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 UI Manager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_UIManager");
                    mInstance = obj.AddComponent<UIManager>();
                    obj.tag = "UIManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<UIManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static UIManager CheckInstanceExist()
    {
        return mInstance;
    }
    #endregion

    #region Observer
    public List<IObserver> registeredObserver = new List<IObserver>();
    public List<ISubject> registeredSubject = new List<ISubject>();

    public void RegisterSubject(ISubject subject)
    {
        registeredSubject.Add(subject);
    }

    public void RegisterObserver(IObserver observer)
    {
        registeredObserver.Add(observer);
    }

    public void DeregisterObserver(IObserver observer)
    {
        registeredObserver.Remove(observer);
    }
    #endregion

    #region General Variables
    [Header("General")]
    [HideInInspector] public ControlUIController controlUI;
    [HideInInspector] public TransitionScript transitionUI;
   //StatUIController StatUI;
    #endregion

   void Awake()
    {
        if (UIManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }

        controlUI = GetComponentInChildren<ControlUIController>();
        transitionUI = GetComponentInChildren<TransitionScript>();
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        controlUI.Reset();
        transitionUI.Reset();
    }
}
