using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region General Variables
    [Header("General")]
    public Sprite frame_shaded;

    private Image frame;
    private Sprite frame_default;
    #endregion

    private void Awake()
    {
        frame = GetComponentsInChildren<Image>()[0];
    }

    // Use this for initialization
    void Start()
    {
        frame_default = frame.sprite;
    }

    // -------------------------------- Pointers --------------------------------

    public void OnPointerDown(PointerEventData eventData)
    {
        ChangeSpr(frame_shaded);

        InputManager.Instance.SetIsShooting(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ChangeSpr(frame_default);

        InputManager.Instance.SetIsShooting(false);
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {

    }

    void ChangeSpr(Sprite newSpr)
    {
        frame.sprite = newSpr;
    }
}
