using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopStand : MonoBehaviour, ISubject
{
    #region Observer
    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < UIManager.Instance.registeredObserver.Count; i++)
        {
            UIManager.Instance.registeredObserver[i].OnNotify(type);
        }
    }
    #endregion

    //References
    private Canvas _shopCanvas;
    private Text shopText;
    private Animator _animator;
    private Animator _weaponAnimator;
    private SpriteRenderer shopWeaponSprite;

    //Variables
    private bool _isPurchased;
    private bool showText;
    
    public Weapon _weapon;
    public string purchaseString;
    public string equipString;
    public string weaponPurchasedString;
    public string weaponEquippedString;
    public string fundsString;

    [Range(0f, 0.3f)] public float textSpeed;

    private void Start()
    {
        //Set references
        _shopCanvas = GetComponentsInChildren<Canvas>()[0];
        shopText = GetComponentsInChildren<Text>()[0];
        shopWeaponSprite = GetComponentsInChildren<SpriteRenderer>()[1];
        _animator = GetComponent<Animator>();
        _weaponAnimator = GetComponentsInChildren<Animator>()[1];

        UIManager.Instance.RegisterSubject(this);

        shopWeaponSprite.sprite = _weapon.sprite;
        _isPurchased = false; // Temporary! Set to SaveManager's value in the future!
        ShopAnimation();

        showText = false;

        StartCoroutine(DisableShopUI(0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_INTERACT_BUTTON);
            EnableShopUI();
            StartCoroutine(WaitForInteract());
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !showText)
        {
            showText = true;

            if (_isPurchased)
            {
                StartCoroutine(DisplayTextWithDelay(equipString, textSpeed));
                //DisplayText(equipString);
            }

            else
            {
                StartCoroutine(DisplayTextWithDelay(purchaseString, textSpeed));
                //DisplayText(purchaseString);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
            StartCoroutine(DisableShopUI(0.1f));
            showText = false;
        }
    }

    void EnableShopUI()
    {
        _shopCanvas.enabled = true;
    }

    IEnumerator DisableShopUI(float duration)
    {
        yield return new WaitForSeconds(duration);
        _shopCanvas.enabled = false;
    }

    IEnumerator DisplayTextWithDelay(string textToDisplay, float delay)
    {
        string currText = "";
        for(int i = 0; i < textToDisplay.Length + 1; i++)
        {
            currText = textToDisplay.Substring(0, i);
            shopText.text = currText;
            yield return new WaitForSeconds(delay);
        }
    }

    void ShopAnimation()
    {
        if(_isPurchased)
        {
            _animator.SetTrigger("Purchased");
            _weaponAnimator.SetTrigger("Purchased");
        }

        else
        {
            _animator.SetTrigger("NotPurchased");
        }
    }

    void DisplayText(string textToDisplay)
    {
        shopText.text = textToDisplay;
    }

    void InsufficientFunds()
    {
        SoundManager.instance.playSingle(SoundManager.instance.insufficientFunds);
        StartCoroutine(DisplayTextWithDelay(fundsString, textSpeed));
        //DisplayText(fundsString);
    }

    void EquipWeapon()
    {
        SoundManager.instance.playSingle(SoundManager.instance.weaponEquipped);
        GameManager.Instance.player.weapon.SetCurrentWeapon(_weapon);
        StartCoroutine(DisplayTextWithDelay(weaponEquippedString, textSpeed));
        //DisplayText(weaponEquippedString);
    }

    IEnumerator PurchaseWeapon()
    {
        if(GameManager.Instance.Coins >= _weapon.cost)
        {
            _isPurchased = true;
            ShopAnimation(); //Play shop animation
            GameManager.Instance.ReduceMoney(_weapon.cost); //Deduct money spent from total amount
            GameManager.Instance.SaveData(); //Save Data
            UIManager.Instance.coinUI.UpdateCoinUI(); //Update coin UI

            SoundManager.instance.playSingle(SoundManager.instance.purchasedSfx); //Play sound effect
            StartCoroutine(DisplayTextWithDelay(weaponPurchasedString, textSpeed)); //Display Text
            //DisplayText(weaponPurchasedString);
            StartCoroutine(WaitForInteract());

            yield return new WaitForSeconds(0.8f);
            showText = false;
        }

        else
        {
            InsufficientFunds();
        }
    }

    IEnumerator WaitForInteract()
    {
        while (!InputManager.Instance.HasInteracted())
        {
            yield return null;
        }

        InputManager.Instance.SetHasInteracted(false);

        if (_isPurchased)
            EquipWeapon();

        else if (!_isPurchased)
            StartCoroutine(PurchaseWeapon());
    }
}
