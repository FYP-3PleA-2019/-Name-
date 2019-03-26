using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityManager : MonoBehaviour, IObserver
{
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

    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < registeredObserver.Count; i++)
        {
            registeredObserver[i].OnNotify(type);
        }
    }
    #endregion

    private static RarityManager mInstance;

    public static RarityManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("RarityManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 Rarity Manager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_RarityManager");
                    mInstance = obj.AddComponent<RarityManager>();
                    obj.tag = "RarityManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<RarityManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static RarityManager CheckInstanceExist()
    {
        return mInstance;
    }

    #region General Variables
    [Header("General")]
    public int rarity;
    #endregion

    void Awake()
    {
        if (RarityManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        RegisterObserver(this);
    }

    public int GetRarity()
    {
        return rarity;
    }

    private void IncraseRarity()
    {
        if (rarity < 10)
            rarity += 1;
    }

    private void DecreaseRarity()
    {
        if (rarity > 0)
            rarity -= 1;
    }

    public void OnNotify(NOTIFY_TYPE type)
    {
        if(type == NOTIFY_TYPE.RARITY_INCREASE)
        {
            IncraseRarity();
        }
        else if(type == NOTIFY_TYPE.RARITY_DECREASE)
        {
            DecreaseRarity();
        }
    }
}
