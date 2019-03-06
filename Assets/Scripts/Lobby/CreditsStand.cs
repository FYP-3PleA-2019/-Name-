using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsStand : MonoBehaviour, ISubject
{
    public void Notify(NOTIFY_TYPE type)
    {
        for(int i = 0; i < UIManager.Instance.registeredObserver.Count; i++)
        {
            UIManager.Instance.registeredObserver[i].OnNotify(type);
        }
    }
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

    public GameObject indicator;
    public string indicatorText;

    bool isOpen;
    #endregion

    #region Unity Functions
    void Awake()
    {
        indicator.GetComponentInChildren<Text>().text = indicatorText;
    }

    private void Start()
    {
        indicator.SetActive(false);
        isOpen = false;
        ResetBlur();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            EnableUI();
            Notify(NOTIFY_TYPE.UI_INTERACT_BUTTON);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (InputManager.Instance.HasInteracted())
            {
                InputManager.Instance.SetHasInteracted(false);
                OpenCredits();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DisableUI());
            Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
        }
    }
    #endregion

    #region Custom Functions
    void OpenCredits()
    {
        AnimationController.Instance.OpenPopUpOneShot(openCreditAnimators, openCreditStrings, ref isOpen);
        StartCoroutine(StartBlur());

        UIManager.Instance.controlUI.HideCanvas();
    }

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

    IEnumerator StartBlur()
    {
        float currBlurSize = panelImage.material.GetFloat("_Size");
        float blurDifference = blurSize - currBlurSize;
        float sizeToIncrease = blurDifference / (blurDuration / Time.deltaTime);

        while (currBlurSize < blurSize)
        {
            panelImage.material.SetFloat("_Size", currBlurSize + sizeToIncrease);
            currBlurSize = panelImage.material.GetFloat("_Size"); ;
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

    void EnableUI()
    {
        indicator.SetActive(true);
    }

    IEnumerator DisableUI()
    {
        indicator.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(0.1f);
        indicator.SetActive(false);
    }
    #endregion
}
