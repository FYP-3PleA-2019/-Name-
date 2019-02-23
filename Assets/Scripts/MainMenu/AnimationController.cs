using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    //This script contains all functions used for playing animations
    #region Singleton
    private static AnimationController mInstance;

    public static AnimationController Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("AnimationController");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 Animation Controller in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_AnimationController");
                    mInstance = obj.AddComponent<AnimationController>();
                    obj.tag = "Animation Controller";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<AnimationController>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static AnimationController CheckInstanceExist()
    {
        return mInstance;
    }

    void Awake()
    {
        if (AnimationController.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Animation Functions
    //Remember to use [StartCoroutine] when using this function
    public IEnumerator PlayAnimationQueue(List<Animator> queueAnimators, List<string> queueTriggerString, List<float> queueAnimationIntervals)
    {
        for(int i = 0; i < queueAnimators.Count; i++)
        {
            queueAnimators[i].SetTrigger(queueTriggerString[i]);

            if(queueAnimationIntervals.Count - 1 < i) //If there is not enough "Animation Intervals" in the inspector, use the previous value.
                yield return new WaitForSeconds(queueAnimationIntervals[i - 1]);

            else
                yield return new WaitForSeconds(queueAnimationIntervals[i]);
        }
    }

    public void PlayAnimationOneShot(List<Animator> oneShotAnimators, List<string> oneShotTriggerString)
    {
        for(int i = 0; i < oneShotAnimators.Count; i++)
        {
            oneShotAnimators[i].SetTrigger(oneShotTriggerString[i]);
        }
    }

    public void ResetAnimationTrigger(List<Animator> animatorsToReset, List<string> triggerStrings)
    {
        for(int i = 0; i < animatorsToReset.Count; i++)
        {
            animatorsToReset[i].ResetTrigger(triggerStrings[i]);
        }
    }
    #endregion

    #region Pop-Up Functions

    #region Pop-Up In Sequence
    public void OpenPopUpInSequence(List<Animator> queueAnimators, List<string> queueTriggerString, List<float> queueAnimationIntervals, ref bool isOpen)
    {
        if (isOpen == true) // Return if pop-up is already opened
            return;

        isOpen = true;

        ResetAnimationTrigger(queueAnimators, queueTriggerString); //Reset the animation triggers
        PlayAnimationQueue(queueAnimators, queueTriggerString, queueAnimationIntervals); //Play Animation
    }

    public void ClosePopUpInSequence(List<Animator> queueAnimators, List<string> queueTriggerString, List<float> queueAnimationIntervals, ref bool isOpen)
    {
        if (isOpen == false) // Return if pop-up is already closed
            return;

        isOpen = false;

        ResetAnimationTrigger(queueAnimators, queueTriggerString); //Reset the animation triggers
        PlayAnimationQueue(queueAnimators, queueTriggerString, queueAnimationIntervals); //Play Animation
    }
    #endregion

    #region Pop-Up One Shot
    public void OpenPopUpOneShot(List<Animator> queueAnimators, List<string> queueTriggerString, ref bool isOpen)
    {
        if (isOpen == true) // Return if pop-up is already opened
            return;

        isOpen = true;

        ResetAnimationTrigger(queueAnimators, queueTriggerString); //Reset the animation triggers
        PlayAnimationOneShot(queueAnimators, queueTriggerString); //Play Animation
    }

    public void ClosePopUpOneShot(List<Animator> queueAnimators, List<string> queueTriggerString, ref bool isOpen)
    {
        if (isOpen == false) // Return if pop-up is already closed
            return;

        isOpen = false;

        ResetAnimationTrigger(queueAnimators, queueTriggerString); //Reset the animation triggers
        PlayAnimationOneShot(queueAnimators, queueTriggerString); //Play Animation
    }
    #endregion

    #endregion 
}
