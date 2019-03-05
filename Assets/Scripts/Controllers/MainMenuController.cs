using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
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
    public GameObject mainMenuCanvas;
    #endregion

    #region Unity Functions
    private void Start()
    {
        if (GameManager.Instance.GetGameState() == GAME_STATE.MAIN_MENU)
        {
            mainMenuCanvas.SetActive(true);
            StartMainMenuAnim();
        }
        else mainMenuCanvas.SetActive(false);
    }
    #endregion

    #region Custom Functions
    
    public void StartMainMenuAnim()
    {
        StartCoroutine(AnimationController.Instance.PlayAnimationQueue(animators, triggerStrings, animIntervals));

        StartCoroutine("WaitForTap");
    }

    IEnumerator WaitForTap()
    {
        while(!animators[animators.Count - 1].GetCurrentAnimatorStateInfo(0).IsName("Tap-To-Start Blinking"))
        {
            yield return null;
        }

        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    {
        GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
        
        AnimationController.Instance.ResetAnimationTrigger(animators, triggerStrings);
        AnimationController.Instance.PlayAnimationOneShot(endingAnimators, endingTriggerStrings);

        yield return new WaitForSeconds(1f);
        mainMenuCanvas.SetActive(false);
    }
    #endregion 
}
