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
    public float minDragDist;
    public float maxDragDist;

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
        touchPoint = joystick.transform.position;
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
        InputManager.Instance.SetIsMoving(true);
        Vector3 dragPoint = eventData.position; //World point
        //Vector3 newTouchPos = Camera.main.ScreenToWorldPoint(eventData.position) //Screen point;

        dragPoint.x -= touchPoint.x;
        dragPoint.y -= touchPoint.y;

        dragPoint = Vector3.ClampMagnitude(new Vector3(dragPoint.x, dragPoint.y, 0f), maxDragDist) + touchPoint;
        joystick.transform.position = new Vector3(dragPoint.x, dragPoint.y, joystick.transform.position.z);
        
        CalculateDir(touchPoint, dragPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        joystick.transform.position = new Vector3(touchPoint.x, touchPoint.y, joystick.transform.position.z);

        InputManager.Instance.SetIsMoving(false);
        InputManager.Instance.SetMoveDir(new Vector3(0, 0, 0));
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        joystick.transform.position = new Vector3(touchPoint.x, touchPoint.y, joystick.transform.position.z);
        //HideJoystick();
    }

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

    private void CalculateDir(Vector3 touchPoint, Vector3 dragPoint)
    {
        Vector3 dir = dragPoint - touchPoint;

        dir.Normalize();

        InputManager.Instance.SetMoveDir(dir);

        if (InputManager.Instance.CanFreeAim())
            InputManager.Instance.SetShootDir(dir);

        #region //Implementing minimum drag distance
        /*if (Mathf.Abs(direction.x) >= minDragDist ||
            Mathf.Abs(direction.y) >= minDragDist)
        {
            direction.Normalize();
            PlayerInputManager.Instance.SetMoving(true, direction);
            PlayerInputManager.Instance.SetShootDir(direction);
        }
        else
        {
            if(PlayerInputManager.Instance.isMoving)
                PlayerInputManager.Instance.SetMoving(false);
        }*/
        #endregion
    }
}
