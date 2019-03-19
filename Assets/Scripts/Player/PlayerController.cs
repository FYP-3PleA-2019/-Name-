using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISubject
{
    #region Observer
    public List<IObserver> registeredObserver = new List<IObserver>();
    public List<ISubject> registeredSubject = new List<ISubject>();

    public void RegisterSubject(ISubject subject)
    {
        registeredSubject.Add(subject);
    }

    public void RegisterObserver(IObserver observer)
    {
        registeredObserver.Add(observer);
    }

    public void DeregisterObserver(IObserver observer)
    {
        registeredObserver.Remove(observer);
    }

    public void Notify(NOTIFY_TYPE type)
    {
        for (int i = 0; i < UIManager.Instance.registeredObserver.Count; i++)
        {
            registeredObserver[i].OnNotify(type);
        }
    }
    #endregion

    //!!!Player's Stats!!!

    #region Player Variables
    [Header("Player")]
    public float defaultHealth;
    public float moveSpeed;
    /*[HideInInspector]*/ public float currHealth;

    [HideInInspector] public Animator playerAnimator;

    private Rigidbody2D _rBody;
    private bool facingLeft;
    #endregion

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        _rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RegisterSubject(this);
    }

    private void Update()
    {
        if (InputManager.Instance.IsMoving()) Move();
        else
        {
            if (GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME)
                Notify(NOTIFY_TYPE.ENTITY_MOVE);
        }
    }

    // -------------------------------- Setters --------------------------------

    //Set player facing
    void SetFacingLeft(bool facingLeft)
    {
        this.facingLeft = facingLeft;
        playerAnimator.SetBool("facingLeft", facingLeft);
    }

    // -------------------------------- Getters --------------------------------


    // -------------------------------- Checkers --------------------------------

    //Check player facing
    public bool FacingLeft()
    {
        bool result = facingLeft;
        return result;
    }

    // -------------------------------- Functions --------------------------------
    
    public void Reset()
    {
        registeredObserver.Clear();
        registeredSubject.Clear();

        SetFacingLeft(false);
        //currHealth = defaultHealth; //move this to reset game in game manager
    }

    public void Move()
    {
        if(GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME) Notify(NOTIFY_TYPE.ENTITY_IDLE);

        Vector3 moveDir = InputManager.Instance.GetMoveDir();
        Vector3 shootDir = InputManager.Instance.GetShootDir();

        if(moveDir.y < 0f && GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME)
            Notify(NOTIFY_TYPE.ENTITY_MOVE);

        if (InputManager.Instance.CanFreeAim())
        {
            if (moveDir.x < 0 && !FacingLeft()) SetFacingLeft(true);
            else if (moveDir.x > 0 && FacingLeft()) SetFacingLeft(false);
        }
        else
        {
            if (shootDir.x < 0 && !FacingLeft()) SetFacingLeft(true);
            else if (shootDir.x > 0 && FacingLeft()) SetFacingLeft(false);
        }
        
        _rBody.MovePosition(new Vector2(transform.position.x + moveDir.x * moveSpeed * Time.deltaTime,
                                        transform.position.y + moveDir.y * moveSpeed * Time.deltaTime));
    }
    
    public void GetDamage(float damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            //CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 0.5f);
        }
    }

    public void MoveWithPlatform(Vector3 target)
    {
        Vector3 g = new Vector3(transform.position.x + target.x, transform.position.y + target.y, 0);

        transform.position = g;
    }

    //Temporary (For Room Manager)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Room")
        {
            RoomManager.Instance.EnteredRoomChecker(other.gameObject);
        }
    }
}
