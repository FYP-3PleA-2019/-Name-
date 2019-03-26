using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE
{
    LOADING,
    MAIN_MENU,
    LOBBY,
    SHOP,
    IN_GAME,
    PAUSED,
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
    public GAME_STATE currGameState;
    public GAME_STATE prevGameState;
    public PlayerCoreController player;

    private bool isReady;

    //Setter & Getters
    public int HighScore
    {
        get { return _highScore; }
        set
        {
            _highScore = value;
        }
    }
    private int _highScore;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
        }
    }
    private int _score;

    public int Coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
        }
    }
    private int _coins;

    public int GameCoins
    {
        get { return _gameCoins; }
        set
        {
            _gameCoins = value;
        }
    }
    private int _gameCoins;

    //Save
    public string saveDataPath;
    #endregion

    void Awake()
    {
        if (GameManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }

    private void Start()
    {
        LoadSavedData();
        Reset();
    }

    // -------------------------------- Setters --------------------------------

    public void SetGameState(GAME_STATE gameState)
    {
        prevGameState = currGameState;
        currGameState = gameState;
    }

    // -------------------------------- Getters --------------------------------

    public GAME_STATE GetCurrGameState()
    {
        return currGameState;
    }

    public GAME_STATE GetPrevGameState()
    {
        return prevGameState;
    }

    public bool GetIsReady()
    {
        return isReady;
    }
    
    // -------------------------------- Functions --------------------------------

    public void Reset()
    { 
        InputManager.Instance.Reset();
        RoomManager.Instance.Reset();
        UIManager.Instance.Reset();
        player.Reset();
    }

    void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCoreController>();
        DontDestroyOnLoad(player);

        currGameState = GAME_STATE.LOADING;
        SetGameState(GAME_STATE.LOADING);

        isReady = true;
    }

    //Save System
    void LoadSavedData()
    {
        GameData data = SaveSystem.LoadSavedData(saveDataPath); //Load Saved Data from [GameData]
        _highScore = data.HighScore;
        _coins = data.Coins;
    }

    public void SaveData()
    {
        string pathName = saveDataPath;
        SaveSystem.SaveAllData(this, pathName);
    }

    //Score & Money
    public void ReceiveMoney(int amount)
    {
        _coins += amount;
    }

    public void ReduceMoney(int amount)
    {
        _coins -= amount;
    }
}
