using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserCannonState
{
    Idle,
    Active,
    Charging,
    Shooting
}

public class LaserCannon : MonoBehaviour
{
    #region GameObject References
    public LineRenderer laserRenderer;
    protected Transform target;
    protected Animator _animator;
    #endregion 

    #region Componenet's Variables
    [Header("Component's Variables")]
    //Timers
    public float activeRange;
    public float cannonCooldown;
    protected float cooldownTimer;
    public float chargingDuration;
    protected float chargingTimer;
    public float shootDuration;
    protected float shootTimer;

    [Space(3)]
    [Header("Attacking Variables")]
    public Transform attackPoint;
    public float attackRange;

    private RaycastHit2D hitInfo;

    [Tooltip("0 = Up, 1 = Down, 2 = Left, 3 = Right")]
    public int attackDir;

    public LaserCannonState state;
    #endregion

    #region Unity Functions
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Setting player as target
        _animator = gameObject.GetComponent<Animator>(); //Assigning animator

        state = LaserCannonState.Idle; //Set beginning state to Idle

        SetLineRendererDirection();

        ResetTimers();
        ResetChildComponents();
    }
    
    void Update()
    {
        if (target == null)
            return;
        
        switch(state)
        {
            case LaserCannonState.Active:
                Active();
                break;

            case LaserCannonState.Charging:
                Charging();
                break;

            case LaserCannonState.Shooting:
                Shooting();
                break;

            default:
                Idle();
                break;
        }
    }
    #endregion

    #region Laser Cannon Behaviour
    void Idle()
    {
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position); //Calculate distance to target

        if (distanceToTarget <= activeRange) //If target is within range, state = active
            state = LaserCannonState.Active;
    }

    void Active()
    {
        // _animator.SetTrigger("Active"); //Play Activate animation

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0.0f)
            state = LaserCannonState.Charging;
    }

    void Charging()
    {
        //_animator.SetTrigger("Charging"); //Play Charging animation

        chargingTimer -= Time.deltaTime;
        if (chargingTimer <= 0.0f)
        {
            laserRenderer.enabled = true;
            state = LaserCannonState.Shooting;
        }
    }

    void Shooting()
    {
        //_animator.SetTrigger("Shooting"); //Play Shooting animation
        
        shootTimer -= Time.deltaTime;
        //Raycast
        hitInfo = Physics2D.Raycast(attackPoint.position, AssignDirection(attackPoint), attackRange);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player")) //Destroy player if player is detected (Just for testing)
                Destroy(hitInfo.collider.gameObject);
        }

        if(shootTimer <= 0.0f)
        {
            ResetTimers();
            ResetChildComponents();
            state = LaserCannonState.Idle;
        }
    }
    #endregion

    #region Custom Functions
    public void ResetTimers()
    {
        //Setting all timers to default value
        cooldownTimer = cannonCooldown;
        chargingTimer = chargingDuration;
        shootTimer = shootDuration;
    }

    public void ResetChildComponents()
    {
        laserRenderer.enabled = false;
    }

    public void SetLineRendererDirection()
    {
        laserRenderer.SetPosition(0, attackPoint.position);
        laserRenderer.SetPosition(1, attackPoint.position + AssignDirection(attackPoint) * attackRange);
    }

    public Vector3 AssignDirection(Transform attackPos)
    {
        Vector3 tempDir = new Vector3(0.0f, 0.0f, 0.0f);

        if (attackDir == 0) //Up
            tempDir = attackPos.up;

        else if (attackDir == 1) //Down
            tempDir = -attackPos.up;

        else if (attackDir == 2) //Left
            tempDir = -attackPos.right;

        else //Right
            tempDir = attackPos.right;

        return tempDir;
    }
    #endregion 
}
