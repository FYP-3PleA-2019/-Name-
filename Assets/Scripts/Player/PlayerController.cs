﻿using System.Collections;
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
    public GameObject guidingArrow;

    public GameObject weaponHolder;
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
        if(InputManager.Instance.IsMoving())
        {
            if (InputManager.Instance.CanControl() && InputManager.Instance.CanMove())
                Move();
        }
        else if(!InputManager.Instance.IsMoving())
        {
            playerAnimator.SetBool("Running", false);

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
        playerAnimator.SetBool("Running", true);

        if (GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME) Notify(NOTIFY_TYPE.ENTITY_IDLE);

        Vector3 moveDir = InputManager.Instance.GetMoveDir();
        Vector3 shootDir = InputManager.Instance.GetShootDir();

        if(GameManager.Instance.GetCurrGameState() == GAME_STATE.IN_GAME)
        {
            if (moveDir.y > 0f)
                Notify(NOTIFY_TYPE.ENTITY_IDLE);

            else if (moveDir.y <= 0f)
                Notify(NOTIFY_TYPE.ENTITY_MOVE);
        }
       
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
        //if (GameManager.Instance.currGameState != GAME_STATE.IN_GAME)
           //return;

        if (_currHealth <= 0)
            return;

        _currHealth -= damage;
        StartCoroutine(TemporarySpriteColorEffect(Color.red, .25f));
        KnockBack(knockBackDir, knockBackForce, knockBackDuration);
        healthBar.GetComponent<HealthBar>().UpdateHealthBar();

        if (_currHealth <= 0)
        {
            GameManager.Instance.SetGameState(GAME_STATE.PAUSED);

            //Disable Inputs
            InputManager.Instance.SetCanControl(false);
            UIManager.Instance.controlUI.HideCanvas();

            //Close health bar and disable it
            healthBar.GetComponent<HealthBar>().DeactivateHealthBar();
            StartCoroutine(DisableHealthBar(0.5f));

            playerAnimator.SetTrigger("StartTeleport");
            weaponHolder.SetActive(false);

            //Notify Game Over
            Notify(NOTIFY_TYPE.GAME_OVER);
        }
    }

    public void MoveWithPlatform(Vector3 target)
    {
        Vector3 tempPos = new Vector3(transform.position.x + target.x, transform.position.y + target.y, 0);

        transform.position = tempPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            InputManager.Instance.SetIsMoving(false);
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

    public void DisableScoreBar(float delay)
    {
        StartCoroutine(DisableScoreBarRoutine(delay));
    }

    private IEnumerator DisableScoreBarRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        scoreBar.SetActive(false);
    }

    //Guiding Arrow
    public void EnableGuidingArrow()
    {
        guidingArrow.SetActive(true);
    }

    public void DisableGuidingArrow(float delay)
    {
        StartCoroutine(DisableGuidingArrowRoutine(delay));
    }

    private IEnumerator DisableGuidingArrowRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        guidingArrow.SetActive(false);
    }

    //
    IEnumerator TeleportedAnimation()
    {
        yield return new WaitForSeconds(.5f);
        playerAnimator.SetTrigger("Teleported");

        yield return new WaitForSeconds(0.35f);
        weaponHolder.SetActive(true);
    }

    public void PlayTeleportAnimation()
    {
        StartCoroutine(TeleportedAnimation());
    }
}
