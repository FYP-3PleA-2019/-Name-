using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IObserver
{
    enum BUTTON_TYPE
    {
        SHOOT,
        INTERACT,
    };

    #region General Variables
    [Header("General")]
    public Sprite frame_onHold;
    public Sprite icon_shoot;
    public Sprite icon_interact;
    
    public Image frame;
    public Image icon;
    public Sprite frame_default;

    private BUTTON_TYPE buttonType;
    #endregion

    private void Awake()
    {
        frame = GetComponentsInChildren<Image>()[0];
        icon = GetComponentsInChildren<Image>()[1];
    }

    // Use this for initialization
    void Start()
    {
        UIManager.Instance.RegisterObserver(this);

        frame_default = frame.sprite;

        buttonType = BUTTON_TYPE.SHOOT;
    }

    // -------------------------------- Pointers --------------------------------

    public void OnPointerDown(PointerEventData eventData)
    {
        frame.sprite = frame_onHold;

        if (buttonType == BUTTON_TYPE.SHOOT)
            InputManager.Instance.SetIsShooting(true);

        else
            InputManager.Instance.SetHasInteracted(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        frame.sprite = frame_default;

        if (buttonType == BUTTON_TYPE.SHOOT)
            InputManager.Instance.SetIsShooting(false);

        else
            InputManager.Instance.SetHasInteracted(false);
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        buttonType = BUTTON_TYPE.SHOOT;
        frame.sprite = frame_default;
        SwitchIcon(icon_shoot);
    }

    public void SwitchIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void OnNotify(NOTIFY_TYPE type)
    {
        if (type == NOTIFY_TYPE.UI_SHOOT_BUTTON)
        {
            SwitchIcon(icon_shoot);
            buttonType = BUTTON_TYPE.SHOOT;
        }
        else if(type == NOTIFY_TYPE.UI_INTERACT_BUTTON)
        {
            SwitchIcon(icon_interact);
            buttonType = BUTTON_TYPE.INTERACT;
        }
    }
}
