using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private static InputManager mInstance;

    public static InputManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject[] tempObjectList = GameObject.FindGameObjectsWithTag("InputManager");

                if (tempObjectList.Length > 1)
                {
                    Debug.LogError("You have more than 1 Input Manager in the Scene");
                }
                else if (tempObjectList.Length == 0)
                {
                    GameObject obj = new GameObject("_InputManager");
                    mInstance = obj.AddComponent<InputManager>();
                    obj.tag = "InputManager";
                }
                else
                {
                    if (tempObjectList[0] != null)
                    {
                        mInstance = tempObjectList[0].GetComponent<InputManager>();
                    }
                }
                DontDestroyOnLoad(mInstance.gameObject);
            }
            return mInstance;
        }
    }

    public static InputManager CheckInstanceExist()
    {
        return mInstance;
    }

    #region General Variables
    [Header("General")]
    public bool canControl;
    public bool canMove;
    public bool canShoot;
    public bool isMoving;
    public bool isShooting;

    public PlayerCoreController player;
    public Vector3 moveDir;
    public Vector3 shootDir;
    #endregion

    #region Joystick Variables
    [Header("Joystick")]
    public float minDragDist;
    public float maxDragDist;
    #endregion

    void Awake()
    {
        if (InputManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }

        player = GameManager.Instance.player;
    }

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (isMoving) player.controller.Move();
        if (isShooting) player.weapon.Shoot();
    }

    // -------------------------------- Setters --------------------------------

    //Set all controls
    void SeCanControl(bool canControl)
    {
        this.canControl = canControl;
    }

    //Set move control
    void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    //Set shoot control
    void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    //Set moving boolean
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;

        if (!isMoving) moveDir = new Vector3(0, 0, 0);

        if (!IsInputting()) player.crosshair.DecreaseAlpha();
        else player.crosshair.IncreaseAlpha();
    }

    //Set shooting boolean
    public void SetIsShooting(bool isShooting)
    {
        this.isShooting = isShooting;

        if (!IsInputting()) player.crosshair.DecreaseAlpha();
        else player.crosshair.IncreaseAlpha();
    }

    //Set move direction
    public void SetMoveDir(Vector3 direction)
    {
        moveDir = direction;
    }

    //Set shoot direction
    public void SetShootDir(Vector3 direction)
    {
        shootDir = direction;
        player.crosshair.Move();
        player.weapon.Rotate();
    }

    // -------------------------------- Getters --------------------------------
    
    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    public Vector3 GetShootDir()
    {
        return shootDir;
    }

    // -------------------------------- Checkers --------------------------------
   
    //Check if all controls are disabled or not
    public bool CanControl()
    {
        bool result = canControl;
        return result;
    }

    //Check if move control is disabled or not
    public bool CanMove()
    {
        bool result = canMove;
        return result;
    }

    //Check if shoot control is disabled or not
    public bool CanShoot()
    {
        bool result = canShoot;
        return result;
    }
    
    //Check if there are basic (movement or shooting) inputs
    public bool IsInputting()
    {
        if (!IsShooting() && !IsMoving()) return false;
        else return true;
    }

    //To check if player is moving
    public bool IsMoving()
    {
        bool result = isMoving;
        return result;
    }

    //To check if player is shooting
    public bool IsShooting()
    {
        bool result = isShooting;
        return result;
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        canControl = true;
        canMove = true;
        canShoot = true;

        isMoving = false;
        isShooting = false;

        moveDir = new Vector3(0.0f, 0.0f, 0.0f);
        shootDir = new Vector3(1.0f, 0.0f, 0.0f);
    }

    public void CalculateDir(Vector3 touchPoint, Vector3 dragPoint)
    {
        Vector3 dir = dragPoint - touchPoint;

        dir.Normalize();

        SetMoveDir(dir);
        SetShootDir(dir);
        
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
