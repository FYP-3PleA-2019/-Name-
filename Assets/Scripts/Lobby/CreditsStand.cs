using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsStand : MonoBehaviour
{
    #region Variables

    //Bluring Variables
    public Image panelImage;
    public Color startingColor;
    public Color targetColor;
    public float blurSize;
    public float blurDuration;

    //Credits Base
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
        ResetBlur();
    }

    private void OnMouseDown()
    {
        AnimationController.Instance.OpenPopUpOneShot(openCreditAnimators, openCreditStrings, ref isOpen);
        StartCoroutine(StartBlur());

        UIManager.Instance.controlUI.HideCanvas();
    }

    IEnumerator StartBlur()
    {
        float currBlurSize = panelImage.material.GetFloat("_Size");
        float blurDifference = blurSize - currBlurSize;
        float sizeToIncrease = blurDifference / (blurDuration / Time.deltaTime);

        while (currBlurSize < blurSize)
        {
            panelImage.material.SetFloat("_Size", currBlurSize + sizeToIncrease);
            currBlurSize = panelImage.material.GetFloat("_Size");;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator EndBlur()
    {
        float currBlurSize = panelImage.material.GetFloat("_Size");
        float sizeToIncrease = blurSize / (blurDuration / Time.deltaTime);

        while (currBlurSize > 0.0f)
        {
            panelImage.material.SetFloat("_Size", currBlurSize - sizeToIncrease);
            currBlurSize = panelImage.material.GetFloat("_Size"); ;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    #endregion

    #region Custom Functions
    public void CloseCredits()
    {
        AnimationController.Instance.ClosePopUpOneShot(closeCreditAnimators, closeCreditStrings, ref isOpen);
        StartCoroutine(EndBlur());

        UIManager.Instance.controlUI.ShowCanvas();
    }

    void ResetBlur()
    {
        panelImage.material.SetColor("_Color", startingColor);
        panelImage.material.SetFloat("_Size", 0.0f);
    }
    #endregion
}
