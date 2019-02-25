using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE
{
    LOADING,
    MAIN_MENU,
    LOBBY,
    PAUSED,
    IN_GAME,
};

public class GameManager : MonoBehaviour {

    private static GameManager mInstance;

    public static GameManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("GameManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 Game Manager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_GameManager");
                    mInstance = obj.AddComponent<GameManager>();
                    obj.tag = "GameManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<GameManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static GameManager CheckInstanceExist()
    {
        return mInstance;
    }

    #region General Variables
    [Header("General")]
    public GAME_STATE gameState;
    public PlayerCoreController player;
    #endregion

    void Awake()
    {
        if (GameManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
        
        SetGameState(GAME_STATE.MAIN_MENU);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerCoreController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
            
    }

    public void SetGameState(GAME_STATE gameState)
    {
        this.gameState = gameState;
    }

    public GAME_STATE GetGameState()
    {
        return gameState;
    }
}
