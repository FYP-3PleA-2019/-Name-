﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_SCENE
{
    LOADING_SCENE,
    LOBBY_SCENE,
    SHOP_SCENE,
    GAME_SCENE,
};

public class CustomSceneManager : MonoBehaviour
{
    private static CustomSceneManager mInstance;

    public static CustomSceneManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("CustomSceneManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 Custom Scene Manager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_CustomSceneManager");
                    mInstance = obj.AddComponent<CustomSceneManager>();
                    obj.tag = "CustomSceneManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<CustomSceneManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static CustomSceneManager CheckInstanceExist()
    {
        return mInstance;
    }

    #region Scene Variables
    //[Header("Scene")]
    //public List<string> scenes;
    #endregion

    void Awake()
    {
        if (CustomSceneManager.CheckInstanceExist())
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

    public void ChangeScene(GameObject player, GAME_SCENE scene)
    {
        Scene sceneName = SceneManager.GetSceneByBuildIndex((int)scene);

        Debug.Log((int)scene);

        SceneManager.LoadScene((int)scene);

        SceneManager.MoveGameObjectToScene(player, sceneName);
    }

    public IEnumerator WaitChangeScene(GAME_SCENE scene, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene((int)scene);
    }
}