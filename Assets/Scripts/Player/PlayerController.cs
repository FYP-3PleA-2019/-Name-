using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    //!!!Player's Stats!!!

    #region Player Variables
    [Header("Player")]
    public float defaultHealth;
    public float moveSpeed;
    [HideInInspector] public float currHealth;

    [HideInInspector] public Animator playerAnimator;
    
    private bool facingLeft;
    #endregion

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        Reset();
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
        SetFacingLeft(false);
        currHealth = defaultHealth;
    }

    public void Move()
    {
        Vector3 moveDir = InputManager.Instance.GetMoveDir();

        if (moveDir.x < 0  && !FacingLeft()) SetFacingLeft(true);
        else if (moveDir.x > 0 && FacingLeft()) SetFacingLeft(false);
        
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    
    public void GetDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            
        }
    }

    //Temporary (For Room Manager)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Room")
            RoomManager.Instance.EnteredRoomChecker(other.gameObject);
    }
}
