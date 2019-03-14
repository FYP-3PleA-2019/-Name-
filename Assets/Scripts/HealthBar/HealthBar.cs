using System.Collections;
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
    private float lerpVal;
    #endregion

    #region Unity Functions 
    private void Start()
    {
        //References
        _animator = GetComponent<Animator>();
        healthBar_Background = GetComponentsInChildren<Image>()[0];
        healthBar_LerpHealth = GetComponentsInChildren<Image>()[1];
        healthBar_CurrHealth = GetComponentsInChildren<Image>()[2];

        //Reset all related components
        Reset();
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.player.controller.currHealth -= 1;
            UpdateHealthBar();
        }
    }
    #endregion 

    #region Custom Functions
    void Reset() //Reset health values
    {
        //Set all health values
        currHealth = GameManager.Instance.player.controller.currHealth;
        totalHealth = currHealth;
        lerpVal = healthBar_LerpHealth.fillAmount;

        //Deactivate fills
        StartCoroutine(DeactivateAfter(0f, healthBar_CurrHealth));
        StartCoroutine(DeactivateAfter(0f, healthBar_LerpHealth));
        StartCoroutine(DeactivateAfter(0f, healthBar_Background));

        //Enable fills after Open animation is done playing
        StartCoroutine(WaitForLoad());
    }

    public void UpdateHealthBar() //Call from player controller when player recieves damage
    {
        currHealth = GameManager.Instance.player.controller.currHealth; //Set currHealth to player's current health
        float calcHealth = currHealth / totalHealth; //Calculate decimal value of health

        healthBar_CurrHealth.fillAmount = calcHealth; //Set fill amount to health

        StartCoroutine(LerpHealthBar(calcHealth));

        //Temporary (Will be called from player)
        if(calcHealth <= 0)
        {
            DeactivateHealthBar();
        }
    }

    public void DeactivateHealthBar()
    {
        StartCoroutine(DeactivateAfter(0f, healthBar_CurrHealth));
        StartCoroutine(DeactivateAfter(0f, healthBar_LerpHealth));
        StartCoroutine(DeactivateAfter(0f, healthBar_Background));

        _animator.SetTrigger("Close");
        //Disable health bar after short delay (0.1f maybe - called from player)
    }

    void ActivateAfter(Image imageToActivate)
    {
        imageToActivate.enabled = true;
    }

    IEnumerator LerpHealthBar(float calcHealth)
    {
        while(lerpVal > calcHealth + 0.005f)
        {
            healthBar_LerpHealth.fillAmount = Mathf.Lerp(healthBar_LerpHealth.fillAmount, calcHealth, Time.deltaTime * lerpSpeed);
            lerpVal = healthBar_LerpHealth.fillAmount;
            yield return null;
        }
    }

    IEnumerator DeactivateAfter(float delay, Image imageToDeactivate)
    {
        yield return new WaitForSeconds(delay);
        imageToDeactivate.enabled = false;
    }

    IEnumerator WaitForLoad() //Activate healthbar fills after Opening animation is finished playing
    {
        while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("HealthBar_Idle"))
        {
            yield return null;
        }

        ActivateAfter(healthBar_Background);
        ActivateAfter(healthBar_CurrHealth);
        ActivateAfter(healthBar_LerpHealth);
    }
    #endregion 
}
