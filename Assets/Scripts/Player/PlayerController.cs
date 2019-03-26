using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Effectors, ISubject
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
        for (int i = 0; i < registeredObserver.Count; i++)
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
    public float CurrHealth
    {
        get { return _currHealth; }
        set
        {
            _currHealth = value;
        }
    }
    private float _currHealth;

    [HideInInspector] public Animator playerAnimator;

    private Rigidbody2D _rBody;
    private bool facingLeft;

    //Health Bar & Score Bar
    public GameObject healthBar;
    public GameObject scoreBar;
    #endregion

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        _rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RegisterSubject(this);
        
        StartCoroutine(DisableScoreBar(Time.deltaTime));
    }

    private void Update()
    {
        if (InputManager.Instance.CanControl() && InputManager.Instance.CanMove() && InputManager.Instance.IsMoving())
            Move();
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
        _currHealth = defaultHealth;

        canKnockBack = true;
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
    
    public void GetDamage(float damage, Vector2 knockBackDir, float knockBackForce, float knockBackDuration)
    {
        if (GameManager.Instance.currGameState != GAME_STATE.IN_GAME)
            return;

        _currHealth -= damage;
        StartCoroutine(TemporarySpriteColorEffect(Color.red, .25f));
        KnockBack(knockBackDir, knockBackForce, knockBackDuration);
        healthBar.GetComponent<HealthBar>().UpdateHealthBar();

        if (_currHealth <= 0)
        {
            //Close health bar and disable it
            healthBar.GetComponent<HealthBar>().DeactivateHealthBar();
            StartCoroutine(DisableHealthBar(0.5f));

            //Notify Game Over
            Notify(NOTIFY_TYPE.GAME_OVER);
        }
    }

    public void MoveWithPlatform(Vector3 target)
    {
        Vector3 tempPos = new Vector3(transform.position.x + target.x, transform.position.y + target.y, 0);

        transform.position = tempPos;
    }

    //Temporary (For Room Manager)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Room")
        {
            RoomManager.Instance.EnteredRoomChecker(other.gameObject);
        }
    }

    //Health Bar
    public void EnableHealthBar()
    {
        healthBar.SetActive(true);
    }

    public IEnumerator DisableHealthBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthBar.SetActive(false);
    }

    //Score Bar
    public void EnableScoreBar()
    {
        scoreBar.SetActive(true);
    }

    public IEnumerator DisableScoreBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        scoreBar.SetActive(false);
    }
}
