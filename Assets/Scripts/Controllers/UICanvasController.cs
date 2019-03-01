using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasController : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    [HideInInspector] public Canvas uiCanvas;
    [HideInInspector] public Joystick joystick;
    [HideInInspector] public MainButton mainButton;
    #endregion

    private void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        joystick = GetComponentInChildren<Joystick>();
        mainButton = GetComponentInChildren<MainButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        joystick.Reset();
        mainButton.Reset();
    }

    public void HideCanvas()
    {
        if (uiCanvas.enabled)
            uiCanvas.enabled = false;
    }

    public void ShowCanvas()
    {
        if (!uiCanvas.enabled)
            uiCanvas.enabled = true;
    }
}
