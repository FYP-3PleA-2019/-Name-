using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//!Attach in joystick panel, requires an active image component
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region General Variables
    [Header("Joystick")]
    private float minDragDist;
    private float maxDragDist;

    private Image frame;
    private Image joystick;

    private Vector3 touchPoint;
    #endregion

    private void Awake()
    {
        frame = GetComponentsInChildren<Image>()[1];
        joystick = GetComponentsInChildren<Image>()[2];
    }

    // Use this for initialization
    void Start()
    {
        touchPoint = new Vector3(0f, 0f, 0f);

        minDragDist = InputManager.Instance.minDragDist;
        maxDragDist = InputManager.Instance.maxDragDist;
    }

    // -------------------------------- Pointers --------------------------------
    
    //Move entire joystick to new touch point
    public void OnPointerDown(PointerEventData eventData)
    {
        touchPoint = eventData.position; //World point
        //touchPos = Camera.main.ScreenToWorldPoint(eventData.position); //Screen point

        frame.transform.position = new Vector3(touchPoint.x, touchPoint.y, frame.transform.position.z);

        ShowJoystick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HideJoystick();
    }

    // -------------------------------- Drags --------------------------------
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        InputManager.Instance.SetIsMoving(true);
    }

    //Calculate drag point from touch point
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dragPoint = eventData.position; //World point
        //Vector3 newTouchPos = Camera.main.ScreenToWorldPoint(eventData.position) //Screen point; 
        
        dragPoint.x -= touchPoint.x;
        dragPoint.y -= touchPoint.y;

        dragPoint = Vector3.ClampMagnitude(new Vector3(dragPoint.x, dragPoint.y, 0f), maxDragDist) + touchPoint;
        joystick.transform.position = new Vector3(dragPoint.x, dragPoint.y, joystick.transform.position.z);
        
        InputManager.Instance.CalculateDir(touchPoint, dragPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        joystick.transform.position = new Vector3(touchPoint.x, touchPoint.y, joystick.transform.position.z);

        InputManager.Instance.SetIsMoving(false);
    }

    // -------------------------------- Functions --------------------------------

    public void ShowJoystick()
    {
        if (!frame.enabled) frame.enabled = true;
        if (!joystick.enabled) joystick.enabled = true;
    }

    public void HideJoystick()
    {
        if (frame.enabled) frame.enabled = false;
        if (joystick.enabled) joystick.enabled = false;
    }
}
