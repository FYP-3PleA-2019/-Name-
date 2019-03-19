using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIController : MonoBehaviour
{
    #region Variables
    //Score and Coins
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
        }
    }
    private int _score;

    public int HighScore
    {
        get { return _highScore; }
        set
        {
            _highScore = value;
        }
    }
    private int _highScore;

    public int Coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
        }
    }
    private int _coins;

    //ScoreBoard 
    [Header("ScoreBoard")]
    private GameObject scoreBoard;
    private List<Text> scoreBoardText = new List<Text>(new Text[3]); //Initialize a list with 3 elements

    //Results
    [Space(5)]
    [Header("Results")]
    private GameObject results;
    private Image resultsBoardImage;
    [Range(0, 5)] public float blurSize;
    [Range(0, 2)] public float blurDuration;

    //UI Animators
    private Animator _scoreBoardAnimator;
    private Animator _resultBoardAnimator;
    #endregion

    #region Unity Functions
    private void Start()
    {
        //Assign references
        _scoreBoardAnimator = GetComponentsInChildren<Animator>()[0];
        _resultBoardAnimator = GetComponentsInChildren<Animator>()[2];
        scoreBoard = transform.GetChild(0).gameObject;
        results = transform.GetChild(1).gameObject;

        //ScoreBoard Texts references
        scoreBoardText[0] = GameObject.FindWithTag("Player").GetComponentsInChildren<Text>()[0];
        scoreBoardText[1] = GetComponentsInChildren<Text>()[0];
        scoreBoardText[2] = GetComponentsInChildren<Text>()[1];

        //Results Board Image references
        resultsBoardImage = GetComponentsInChildren<Image>()[2];
        
        //Disable results canvas
        results.SetActive(false);

        //Score Board Initialization
        InitializeScores();
        UpdateScoreBoard();

        //Blur
        UIManager.Instance.blurUI.DisableCanvas();
        UIManager.Instance.blurUI.BlurDuration = blurDuration;
        UIManager.Instance.blurUI.BlurSize = blurSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(ShowResults());
    }
    #endregion

    #region Score/Result Function
    public IEnumerator ShowResults() //Call this after player died
    {
        UIManager.Instance.blurUI.EnableCanvas();
        StartCoroutine(UIManager.Instance.blurUI.StartBlur());

        //Wait for screen to fully blur
        yield return new WaitForSeconds(blurDuration);
        StartCoroutine(CloseScoreBoard());
        EnableResults();

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

        //Un-blur
        StartCoroutine(UIManager.Instance.blurUI.EndBlur());

        //Play Results Closing animation
        _resultBoardAnimator.SetTrigger("Close");

        //Wait for X amount of time before scene transition
        yield return new WaitForSeconds(0.5f);

        //Scene transition
        UIManager.Instance.transitionUI.PlayTransitionAnimation(0);
        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
        CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 1.5f);
    }
    #endregion 

    #region ScoreBoard
    void InitializeScores() //Only called at Start
    {
        _highScore = 0; //Get actual high score from manager or something;
        _score = 0; //Get actual score from manager
        _coins = 0; //Get actual coins from manager
    }

    //Called everytime there is changes in score values
    //EXAMPLE : If player picked up a coin
    //_uiController.GetComponent<GameSceneUIController>().Coins = amountOfCoins;
    //UpdateScoreBoard();
    public void UpdateScoreBoard()
    {
        //Score Text
        scoreBoardText[0].text = _score + "m";

        //HighScore Text
        scoreBoardText[1].text = "BEST    " + _highScore + "m";

        //Coin Text
        scoreBoardText[2].text = "" + _coins;
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
}