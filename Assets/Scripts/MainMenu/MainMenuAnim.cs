﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnim : MonoBehaviour
{
    #region Variables
    [Space(3)]
    [Header("Main Menu Beinning Animations")]
    public List<Animator> animators;
    public List<string> triggerStrings;
    public List<float> animIntervals;

    [Space(3)]
    [Header("Main Menu Closing Animations")]
    public List<Animator> endingAnimators;
    public List<string> endingTriggerStrings;

    [Space(3)]
    [Header("Canvas")]
    public Canvas mainMenuCanvas;
    public Canvas creditsCanvas;
    public Canvas uiCanvas;

    bool hasStarted;
    #endregion

    #region Unity Functions
    private void Start()
    {
        //hasStarted = false;
        creditsCanvas.enabled = false;
        uiCanvas.enabled = false;

        StartMainMenuAnim();
    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.Instance.GetGameState() != GAME_STATE.MAIN_MENU)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("StartGame");
        }
    }
    #endregion
        
    #region Custom Functions

    IEnumerator StartGame()
    {
        //hasStarted = true;
        AnimationController.Instance.ResetAnimationTrigger(animators, triggerStrings);
        AnimationController.Instance.PlayAnimationOneShot(endingAnimators, endingTriggerStrings);

        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);

        yield return new WaitForSeconds(1f);
        mainMenuCanvas.enabled = false;
        creditsCanvas.enabled = true;
        uiCanvas.enabled = true;
    }

    public void StartMainMenuAnim()
    {
        StartCoroutine(AnimationController.Instance.PlayAnimationQueue(animators, triggerStrings, animIntervals));
    }
    #endregion 
}