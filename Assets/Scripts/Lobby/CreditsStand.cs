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
    [Range(0, 5)] public float blurSize;
    [Range(0, 2)] public float blurDuration;

    //Credits Base
    public List<Animator> openCreditAnimators;
    public List<string> openCreditStrings;

    public List<Animator> closeCreditAnimators;
    public List<string> closeCreditStrings;

    public GameObject indicator;
    public string indicatorText;

    public Animator blinkTextAnimator;

    [Space(5)]
    [Header("Arrow Indicator")]
    public GameObject arrowIndicator;
    public Sprite indicatorSprite;

    bool isOpen;
    #endregion

    #region Unity Functions
    void Awake()
    {
        indicator.GetComponentInChildren<Text>().text = indicatorText;
    }

    private void Start()
    {
        //Blur
        UIManager.Instance.blurUI.DisableCanvas();
        UIManager.Instance.blurUI.Reset();
        UIManager.Instance.blurUI.BlurDuration = blurDuration;
        UIManager.Instance.blurUI.BlurSize = blurSize;

        indicator.SetActive(false);
        isOpen = false;
        InstantiateArrowIndicator();
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
                StartCoroutine(OpenCredits());
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
    void InstantiateArrowIndicator()
    {
        GameObject indicator = Instantiate(arrowIndicator, transform.position, Quaternion.identity);
        indicator.transform.SetParent(this.transform);
        indicator.GetComponent<ArrowIndicator>().Target = this.transform;
        indicator.GetComponent<ArrowIndicator>().SpriteToDisplay = indicatorSprite;
    }

    IEnumerator OpenCredits()
    {
        AnimationController.Instance.OpenPopUpOneShot(openCreditAnimators, openCreditStrings, ref isOpen);
        StartCoroutine(UIManager.Instance.blurUI.StartBlur());

        UIManager.Instance.controlUI.HideCanvas();
        UIManager.Instance.blurUI.EnableCanvas();

        yield return new WaitForSeconds(.7f);
        blinkTextAnimator.SetTrigger("Blink");

        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        StartCoroutine(CloseCredits());
    }

    IEnumerator CloseCredits()
    {
        AnimationController.Instance.ClosePopUpOneShot(closeCreditAnimators, closeCreditStrings, ref isOpen);
        StartCoroutine(UIManager.Instance.blurUI.EndBlur());

        yield return new WaitForSeconds(0.75f);
        UIManager.Instance.controlUI.ShowCanvas();
        blinkTextAnimator.SetTrigger("StopBlink");
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
