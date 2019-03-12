using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    #region Transition Animation
    [Space(5)]
    [Header("Transition Animation")]
    private Canvas transitionCanvas;
    private Animator transitionAnimator;
    public List<string> triggerStrings;
    #endregion

    private void Awake()
    {
        transitionCanvas = GetComponent<Canvas>();
        transitionAnimator = GetComponent<Animator>();
    }

    public void PlayTransitionAnimation(int index)
    {
        if (index == 0) //Enter
        {
            AnimationController.Instance.PlaySingularTriggerAnimation(transitionAnimator, triggerStrings[0]);
        }

        else //Leave
        {
            AnimationController.Instance.PlaySingularTriggerAnimation(transitionAnimator, triggerStrings[1]);
        }
    }

    public void Reset()
    {
        AnimationController.Instance.ResetSingularAnimatorTriggers(transitionAnimator, triggerStrings);
    }
}
