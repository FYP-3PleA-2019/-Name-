using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityManager : MonoBehaviour
{
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
    public float f;
    #endregion

    void Awake()
    {
        if (RarityManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
