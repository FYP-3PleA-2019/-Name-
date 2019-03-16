﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables
    [Range(1, 5)] public float lerpSpeed;
    private Animator _animator;
    private Image healthBar_CurrHealth;
    private Image healthBar_LerpHealth;
    private Image healthBar_Background;

    private float currHealth;
    private float totalHealth;
    private float calcHealth; //Decimal Value of currHealth / totalHealth
    private float lerpVal;
    private float healthLerpVal;
    private bool canLerp;
    #endregion

    #region Unity Functions 
    private void OnEnable()
    {
        Reset(); //Reset components
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//Temporary
        {
            GameManager.Instance.player.controller.currHealth -= 1;
            UpdateHealthBar(); //Call this when player is damaged
        }

        if(!ReturnApproximation(calcHealth, lerpVal, 0.001f) && canLerp) //Lerp bars
        {
            LerpBar(calcHealth, ref lerpVal, healthBar_LerpHealth, lerpSpeed);
        }

        if(!ReturnApproximation(calcHealth, healthLerpVal, 0.001f) && canLerp)
        {
            LerpBar(calcHealth, ref healthLerpVal, healthBar_CurrHealth, lerpSpeed);
        }
    }
    #endregion 

    #region Custom Functions
    //==========================Public Functions====================================
    public void Reset() //Reset health bar componenets, call this function when enabling health bar.
    {
        //Set references for this object
        SetReferences();

        //Reset lerp boolean
        canLerp = false;

        //Set all health values
        currHealth = GameManager.Instance.player.controller.currHealth;
        totalHealth = currHealth;

        calcHealth = currHealth / totalHealth;

        //Set all fillAmount to health value
        healthBar_LerpHealth.fillAmount = 0f;
        healthBar_CurrHealth.fillAmount = 0f;
        lerpVal = healthBar_LerpHealth.fillAmount;
        healthLerpVal = healthBar_CurrHealth.fillAmount;

        //Deactivate fills
        healthBar_Background.enabled = false;
        healthBar_CurrHealth.enabled = false;
        healthBar_LerpHealth.enabled = false;

        //Enable fills after Open animation is done playing
        StartCoroutine(WaitForLoad(calcHealth));
    }

    public void UpdateHealthBar() //Call from player controller when player recieves damage
    {
        currHealth = GameManager.Instance.player.controller.currHealth; //Set currHealth to player's current health
        calcHealth = currHealth / totalHealth; //Calculate decimal value of health

        healthBar_CurrHealth.fillAmount = calcHealth; //Set fill amount to health

        //Temporary (Will be called from player)
        if(calcHealth <= 0)
        {
            DeactivateHealthBar();
        }
    }

    public void DeactivateHealthBar()
    {
        healthBar_Background.enabled = false;
        healthBar_CurrHealth.enabled = false;
        healthBar_LerpHealth.enabled = false;

        _animator.SetTrigger("Close");
        //Disable health bar after short delay (0.1f maybe - called from player)
    }
    //===============================================================================

    
    //===============================Private Functions==================================
    void SetReferences()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (healthBar_Background == null)
            healthBar_Background = GetComponentsInChildren<Image>()[0];

        if (healthBar_LerpHealth == null)
            healthBar_LerpHealth = GetComponentsInChildren<Image>()[1];

        if (healthBar_CurrHealth == null)
            healthBar_CurrHealth = GetComponentsInChildren<Image>()[2];
    }

    void LerpBar(float calculatedHealth, ref float lerpValue, Image barToFill, float speed)
    {
        barToFill.fillAmount = Mathf.Lerp(barToFill.fillAmount, calculatedHealth, Time.deltaTime * speed);
        lerpValue = barToFill.fillAmount;
    }

    IEnumerator WaitForLoad(float calculatedHealth) //Activate healthbar fills after Opening animation is finished playing
    {
        while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("HealthBar_Idle"))
        {
            yield return null;
        }

        //Enable all healthbar fills
        healthBar_Background.enabled = true;
        healthBar_CurrHealth.enabled = true;
        healthBar_LerpHealth. enabled = true;

        //Allow fills to start lerping
        canLerp = true;
    }
    //================================================================================================
    #endregion

    #region Tools
    bool ReturnApproximation(float a, float b, float value) //Used for checking if 2 seperate values are similar.
    {
        return (Mathf.Abs(a - b) < value);
    }
    #endregion 
}
