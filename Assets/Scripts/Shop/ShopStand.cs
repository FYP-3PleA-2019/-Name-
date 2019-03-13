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

    public int PurchaseCost
    {
        get { return _purchaseCost; }
        set
        {
            _purchaseCost = value;
        }
    }
    private int _purchaseCost;

    public string WeaponName
    {
        get { return _weaponName; }
        set
        {
            _weaponName = value;
        }
    }
    private string _weaponName;
    
    private bool _isPurchased;

    public Weapon _weapon;

    public Canvas _shopCanvas;
    public Text shopText;
    public string purchaseString;
    public string equipString;
    public string fundsString;
    public string weaponPurchasedString;
    public string weaponEquippedString;

    [Range(0f, 0.3f)] public float textSpeed;

    private string currText = "";

    private void Start()
    {
        UIManager.Instance.RegisterSubject(this);
        _isPurchased = false; // Temporary! Set to SaveManager's value in the future!
        StartCoroutine(DisableShopUI(0.1f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_INTERACT_BUTTON);
            EnableShopUI();

            if (_isPurchased)
            {
                StartCoroutine(DisplayText(equipString, textSpeed));
            }

            else
            {
                StartCoroutine(DisplayText(purchaseString, textSpeed));
            }

            StartCoroutine(WaitForInteract());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Notify(NOTIFY_TYPE.UI_SHOOT_BUTTON);
            StartCoroutine(DisableShopUI(0.1f));
        }
    }

    public void EnableShopUI()
    {
        _shopCanvas.enabled = true;
    }

    public IEnumerator DisableShopUI(float duration)
    {
        yield return new WaitForSeconds(duration);
        _shopCanvas.enabled = false;
    }

    public IEnumerator DisplayText(string textToDisplay, float delay)
    {
        for(int i = 0; i < textToDisplay.Length + 1; i++)
        {
            currText = textToDisplay.Substring(0, i);
            shopText.text = currText;
            yield return new WaitForSeconds(delay);
        }
    }

    public void InsufficientFunds()
    {
        SoundManager.instance.playSingle(SoundManager.instance.insufficientFunds);
        StartCoroutine(DisplayText(fundsString, textSpeed));
    }

    public void EquipWeapon()
    {
        SoundManager.instance.playSingle(SoundManager.instance.weaponEquipped);
        GameManager.Instance.player.weapon.SetCurrentWeapon(_weapon);
        StartCoroutine(DisplayText(weaponEquippedString, textSpeed));
    }

    public void PurchaseWeapon()
    {
        SoundManager.instance.playSingle(SoundManager.instance.purchasedSfx);
        //Check if player's money > weapon's cost
        //If not InsufficientFunds();
        //Else Weapon is Purchased;
        _isPurchased = true;
        StartCoroutine(DisplayText(weaponPurchasedString, textSpeed));
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
            PurchaseWeapon();
    }
}
