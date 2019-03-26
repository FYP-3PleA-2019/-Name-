﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIController : MonoBehaviour, IObserver
{
    #region Variables
    //Score and Coins
    private int _score;
    private int _highScore;
    private int _coins;
    private int _currScore;

    private float furthestDist;

    private Transform player;
    private Vector2 startPos;

    //ScoreBoard 
    [Header("ScoreBoard")]
    private GameObject scoreBoard;
    private List<Text> scoreBoardText = new List<Text>(new Text[3]); //Initialize a list with 3 elements
    private bool newHighScore;

    //Results
    [Space(5)]
    [Header("Results")]
    private GameObject results;
    //private Image resultsBoardImage;
    [Range(0, 5)] public float blurSize;
    [Range(0, 2)] public float blurDuration;

    //UI Animators
    private Animator _scoreBoardAnimator;
    private Animator _resultBoardAnimator;
    #endregion

    #region Unity Functions
    private void Start()
    {
        //Observer
        GameManager.Instance.player.controller.RegisterObserver(this);

        //Disable Lobby/Shop Coin UI
        UIManager.Instance.coinUI.DisableCanvas();

        //Enable Player's HealthBar and ScoreBar
        GameManager.Instance.player.controller.EnableHealthBar();
        GameManager.Instance.player.controller.EnableScoreBar();

        //Assign references
        _scoreBoardAnimator = GetComponentsInChildren<Animator>()[0];
        _resultBoardAnimator = GetComponentsInChildren<Animator>()[2];
        scoreBoard = transform.GetChild(0).gameObject;
        results = transform.GetChild(1).gameObject;

        //ScoreBoard Texts references
        scoreBoardText[0] = GameManager.Instance.player.GetComponentsInChildren<Text>()[0];
        scoreBoardText[1] = GetComponentsInChildren<Text>()[0];
        scoreBoardText[2] = GetComponentsInChildren<Text>()[1];

        //Results Board Image references
        //resultsBoardImage = GetComponentsInChildren<Image>()[2];
        
        //Disable results canvas
        results.SetActive(false);

        //Score Board Initialization
        InitializeScores();
        UpdateScoreBoard();

        //Blur
        UIManager.Instance.blurUI.DisableCanvas();
        UIManager.Instance.blurUI.BlurDuration = blurDuration;
        UIManager.Instance.blurUI.BlurSize = blurSize;

        //Set Player Starting Position Reference
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        startPos = player.transform.position;
        furthestDist = 0f;

        newHighScore = false;
    }

    private void Update()
    {
        CalculateScore();
        UpdateScoreBoard();
    }
    #endregion

    #region Score/Result Function
    public IEnumerator ShowResults() //Call this after player died
    {
        //Check High Score
        CheckHighScore();

        //Disable Input and controls
        InputManager.Instance.SetCanControl(false);
        UIManager.Instance.controlUI.HideCanvas();
        UIManager.Instance.blurUI.EnableCanvas();
        StartCoroutine(UIManager.Instance.blurUI.StartBlur());

        //Wait for screen to fully blur
        yield return new WaitForSeconds(blurDuration);
        StartCoroutine(CloseScoreBoard());
        EnableResults();

        //TO-DO : Show Results Text
        int moneyEarned = GameManager.Instance.GameCoins;
        GameManager.Instance.ReceiveMoney(moneyEarned); //Add earned coins to total amount of coins.
        GameManager.Instance.GameCoins = 0; //Reset game coins value to 0;

        //Wait for results to fully load in
        while (!_resultBoardAnimator.GetCurrentAnimatorStateInfo(0).IsName("Results_Idle"))
        {
            yield return null;
        }

        //If input is detected, 
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        //Save score and coins earned
        GameManager.Instance.SaveData();

        //Un-blur
        StartCoroutine(UIManager.Instance.blurUI.EndBlur());

        //Play Results Closing animation
        _resultBoardAnimator.SetTrigger("Close");

        //Wait for X amount of time before scene transition
        yield return new WaitForSeconds(0.5f);

        //Scene transition
        GameManager.Instance.player.controller.StartCoroutine(GameManager.Instance.player.controller.DisableScoreBar(0.5f));
        UIManager.Instance.transitionUI.PlayTransitionAnimation(0);
        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
        CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 1.5f);
    }
    #endregion 

    #region ScoreBoard
    void InitializeScores() //Only called at Start
    {
        _highScore = GameManager.Instance.HighScore;
        _score = GameManager.Instance.Score;
        _coins = GameManager.Instance.GameCoins; 
    }

    void CalculateScore()
    {
        float distance = Vector2.Distance(startPos, player.position);
        if (distance > furthestDist)
        {
            furthestDist = distance;

            _score = (int)distance;

            if (_highScore < _score)
                newHighScore = true;
        }
    }

    void CheckHighScore()
    {
        if (newHighScore)
            GameManager.Instance.HighScore = _score;
    }
    
    public void UpdateScoreBoard()
    {
        //Score Text
        scoreBoardText[0].text = _score + "m";

        //HighScore Text
        if (!newHighScore)
            scoreBoardText[1].text = "BEST    " + _highScore + "m";

        else
            scoreBoardText[1].text = "NEW BEST " + _score + "m";

        //Coin Text
        scoreBoardText[2].text = "" + GameManager.Instance.GameCoins;
    }

    IEnumerator CloseScoreBoard()
    {
        _scoreBoardAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(2.0f);
        scoreBoard.SetActive(false);
    }
    #endregion

    #region Results
    void EnableResults()
    {
        results.SetActive(true);
    }
    #endregion

    #region Observer
    public void OnNotify(NOTIFY_TYPE type)
    {
        if(type == NOTIFY_TYPE.GAME_OVER)
        {
            StartCoroutine(ShowResults());
        }
    }
    #endregion
}