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
    private Vector3 moveDir;
    private Vector3 shootDir;
    #endregion

    #region Controls Variables
    [Header("Controls")]
    private bool canControl;
    private bool canMove;
    private bool canShoot;
    private bool canInteract;
    private bool canFreeAim;
    private bool isMoving;
    private bool isShooting;
    private bool hasInteracted;
    #endregion

    void Awake()
    {
        if (InputManager.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // -------------------------------- Setters --------------------------------
    
    public void SetCanControl(bool canControl)
    {
        this.canControl = canControl;
    }
    
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    
    public void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
    }
    
    public void SetCanFreeAim(bool canFreeAim)
    {
        this.canFreeAim = canFreeAim;
    }

    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
        
        if (!IsInputting()) GameManager.Instance.player.crosshair.DecreaseAlpha();
        else GameManager.Instance.player.crosshair.IncreaseAlpha();
    }
    
    public void SetIsShooting(bool isShooting)
    {
        this.isShooting = isShooting;

        if (isShooting) GameManager.Instance.player.weapon.OnShootBegin();
        else GameManager.Instance.player.weapon.OnShootEnd();
        
        if (!IsInputting()) GameManager.Instance.player.crosshair.DecreaseAlpha();
        else GameManager.Instance.player.crosshair.IncreaseAlpha();
    }
    
    public void SetHasInteracted(bool hasInteracted)
    {
        this.hasInteracted = hasInteracted;
    }

    public void SetMoveDir(Vector3 direction)
    {
        moveDir = direction;
    }
    
    public void SetShootDir(Vector3 direction)
    {
        shootDir = direction;

        GameManager.Instance.player.crosshair.Move();
        GameManager.Instance.player.weapon.Rotate();
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
   
    public bool CanControl()
    {
        bool result = canControl;
        return result;
    }
    
    public bool CanMove()
    {
        bool result = canMove;
        return result;
    }
    
    public bool CanShoot()
    {
        bool result = canShoot;
        return result;
    }
    
    public bool CanInteract()
    {
        bool result = canInteract;
        return result;
    }

    public bool CanFreeAim()
    {
        bool result = canFreeAim;
        return result;
    }

    public bool IsInputting()
    {
        if (!IsShooting() && !IsMoving()) return false;
        else return true;
    }
    
    public bool IsMoving()
    {
        bool result = isMoving;
        return result;
    }
    
    public bool IsShooting()
    {
        bool result = isShooting;
        return result;
    }

    public bool HasInteracted()
    {
        bool result = hasInteracted;
        return result;
    }

    // -------------------------------- Functions --------------------------------

    public void Reset()
    {
        canControl = true;
        canMove = true;
        canShoot = true;
        canInteract = true;
        canFreeAim = true;

        isMoving = false;
        isShooting = false;
        hasInteracted = false;

        moveDir = new Vector3(0.0f, 0.0f, 0.0f);
        shootDir = new Vector3(1.0f, 0.0f, 0.0f);
    }
}
