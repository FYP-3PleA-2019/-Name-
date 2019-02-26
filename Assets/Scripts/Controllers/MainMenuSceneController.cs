using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    #region Variables
    [Space(3)]
    [Header("Main Menu Beginning Animations")]
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
    #endregion

    #region Unity Functions
    private void Start()
    {
        if (GameManager.Instance.GetGameState() == GAME_STATE.MAIN_MENU)
        {
            creditsCanvas.enabled = false;
            uiCanvas.enabled = false;

            StartMainMenuAnim();
        }
        else
        {
            mainMenuCanvas.enabled = false;
            creditsCanvas.enabled = true;
            uiCanvas.enabled = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.Instance.GetGameState() == GAME_STATE.MAIN_MENU)
        {
            if (animators[animators.Count - 1].GetCurrentAnimatorStateInfo(0).IsName("Tap-To-Start Blinking"))
            {
                if (Input.GetMouseButtonDown(0))
                    StartCoroutine("StartGame");
            }
        }
    }
    #endregion
        
    #region Custom Functions

    IEnumerator StartGame()
    {
        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);

        yield return new WaitForSeconds(0.3f);
        AnimationController.Instance.ResetAnimationTrigger(animators, triggerStrings);
        AnimationController.Instance.PlayAnimationOneShot(endingAnimators, endingTriggerStrings);

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
