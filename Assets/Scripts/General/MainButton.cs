using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainButton : MonoBehaviour, IObserver
{
    enum BUTTON_TYPE
    {
        SHOOT,
        INTERACT,
    };

    #region General Variables
    [Header("General")]
    public Sprite mainButton_frame_onHold;
    public Sprite switchButton_frame_onHold;
    public Sprite mainButton_icon_shoot;
    public Sprite mainButton_icon_interact;

    private BUTTON_TYPE buttonType;

    private Image mainButton_frame;
    private Image mainButton_icon;
    private Image switchButton_frame;
    private Image switchButton_icon;

    private Sprite mainButton_frame_default;
    private Sprite switchButton_frame_default;
    #endregion

    private void Awake()
    {
        mainButton_frame = GetComponentsInChildren<Image>()[0];
        mainButton_icon = GetComponentsInChildren<Image>()[1];
        switchButton_frame = GetComponentsInChildren<Image>()[2];
        switchButton_icon = GetComponentsInChildren<Image>()[3];
    }

    // Use this for initialization
    void Start()
    {
        UIManager.Instance.RegisterObserver(this);
        
        buttonType = BUTTON_TYPE.SHOOT;
        mainButton_frame_default = mainButton_frame.sprite;
        switchButton_frame_default = switchButton_frame.sprite;
        
        if (GameManager.Instance.player.weapon.prevWeapon != null)
        {
            Sprite currWeaponSpr = GameManager.Instance.player.weapon.prevWeapon.GetSprite();
            switchButton_icon.sprite = currWeaponSpr;
        }
    }

    // -------------------------------- Pointers --------------------------------

    public void OnMainButtonDown()
    {
        mainButton_frame.sprite = mainButton_frame_onHold;

        if (buttonType == BUTTON_TYPE.SHOOT)
            InputManager.Instance.SetIsShooting(true);

        else
            InputManager.Instance.SetHasInteracted(true);
    }

    public void OnMainButtonUp()
    {
        mainButton_frame.sprite = mainButton_frame_default;

        if (buttonType == BUTTON_TYPE.SHOOT)
            InputManager.Instance.SetIsShooting(false);

        else
            InputManager.Instance.SetHasInteracted(false);
    }

    public void OnSwitchButtonDown()
    {
        switchButton_frame.sprite = switchButton_frame_onHold;

        GameManager.Instance.player.weapon.SwitchWeapon();

        if(GameManager.Instance.player.weapon.prevWeapon != null)
        {
            Sprite currWeaponSpr = GameManager.Instance.player.weapon.prevWeapon.GetSprite();
            switchButton_icon.sprite = currWeaponSpr;
        }
    }

    public void OnSwitchButtonUp()
    {
        switchButton_frame.sprite = switchButton_frame_default;
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        buttonType = BUTTON_TYPE.SHOOT;

        mainButton_frame.sprite = mainButton_frame_default;
        SwitchMainButtonIcon(mainButton_icon_shoot);
    }

    public void SwitchMainButtonIcon(Sprite sprite)
    {
        mainButton_icon.sprite = sprite;
    }

    public void SwitchSwitchButtonIcon(Sprite sprite)
    {
        switchButton_icon.sprite = sprite;
    }

    public void OnNotify(NOTIFY_TYPE type)
    {
        if (type == NOTIFY_TYPE.UI_SHOOT_BUTTON)
        {
            SwitchMainButtonIcon(mainButton_icon_shoot);
            buttonType = BUTTON_TYPE.SHOOT;
        }
        else if(type == NOTIFY_TYPE.UI_INTERACT_BUTTON)
        {
            SwitchMainButtonIcon(mainButton_icon_interact);
            buttonType = BUTTON_TYPE.INTERACT;
        }
    }
}
