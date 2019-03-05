using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    #region Variables
    public List<Animator> openCreditAnimators;
    public List<string> openCreditStrings;

    public List<Animator> closeCreditAnimators;
    public List<string> closeCreditStrings;

    bool isOpen;
    #endregion

    #region Unity Functions
    private void Start()
    {
        isOpen = false;
    }

    private void OnMouseDown()
    {
        AnimationController.Instance.OpenPopUpOneShot(openCreditAnimators, openCreditStrings, ref isOpen);

        UIManager.Instance.controlUI.HideCanvas();
    }
    #endregion

    #region Custom Functions
    public void CloseCredits()
    {
        AnimationController.Instance.ClosePopUpOneShot(closeCreditAnimators, closeCreditStrings, ref isOpen);

        UIManager.Instance.controlUI.ShowCanvas();
    }
    #endregion
}
