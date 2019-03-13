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

public enum ShootDirection
{
    Up,
    Down,
    Left,
    Right
}

public class LaserCannon : MonoBehaviour
{
    #region GameObject References
    public LineRenderer laserRenderer;
    protected Transform target;
    protected Animator _animator;
    #endregion 

    #region Component's Variables
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
    public LayerMask attackLayer;
    public Transform attackPoint;
    public float attackRange;
    
    public float rotationSpeed;

    private RaycastHit2D hitInfo;

    public LaserCannonState state;
    public ShootDirection _shootDirection;
    #endregion

    #region Unity Functions
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Setting player as target
        _animator = gameObject.GetComponent<Animator>(); //Assigning animator

        state = LaserCannonState.Idle; //Set beginning state to Idle

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
        ResetAllAnimationTriggers();
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position); //Calculate distance to target

        if (distanceToTarget <= activeRange) //If target is within range, state = active
            state = LaserCannonState.Active;
    }

    void Active()
    {
         _animator.SetTrigger("Activate"); //Play Activate animation

        FaceTarget();

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0.0f)
            state = LaserCannonState.Charging;
    }

    void Charging()
    {
        _animator.SetTrigger("Charge"); //Play Charging animation

        chargingTimer -= Time.deltaTime;
        if (chargingTimer <= 0.0f)
        {
            laserRenderer.enabled = true;
            state = LaserCannonState.Shooting;
        }
    }

    void Shooting()
    {
        _animator.SetTrigger("Shoot"); //Play Shooting animation
        
        shootTimer -= Time.deltaTime;

        SetLineRendererDirection(transform.right);

        //Raycast
        hitInfo = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, attackLayer);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player")) //Destroy player if player is detected (Just for testing)
            {
                //TEMPORARY
                GameManager.Instance.SetGameState(GAME_STATE.LOBBY);
                UIManager.Instance.transitionUI.PlayTransitionAnimation(0);

                CustomSceneManager.Instance.LoadSceneWait(GAME_SCENE.LOBBY_SCENE, 1.5f);
            }
        }

        if(shootTimer <= 0.0f)
        {
            _animator.SetTrigger("Deactivate"); //Play deactivate animation after finish shooting
            ResetTimers();
            ResetChildComponents();
            state = LaserCannonState.Idle;
        }
    }
    #endregion

    #region Custom Functions
    Vector3 ReturnShootDirection()
    {
        Vector3 tempDir;

        if (_shootDirection == ShootDirection.Up)
            tempDir = new Vector3(transform.position.x, transform.position.y + attackRange, 0.0f);

        else if(_shootDirection == ShootDirection.Down)
            tempDir = new Vector3(transform.position.x, transform.position.y - attackRange, 0.0f);

        else if(_shootDirection == ShootDirection.Left)
            tempDir = new Vector3(transform.position.x - attackRange, transform.position.y, 0.0f);

        else
            tempDir = new Vector3(transform.position.x + attackRange, transform.position.y, 0.0f);

        return tempDir;
    }

    void FaceTarget()
    {
        Vector3 direction = ReturnShootDirection() - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }


    public void ResetTimers()
    {
        //Setting all timers to default value
        cooldownTimer = cannonCooldown;
        chargingTimer = chargingDuration;
        shootTimer = shootDuration;
    }

    public void ResetAllAnimationTriggers()
    {
        _animator.ResetTrigger("Activate");
        _animator.ResetTrigger("Charge");
        _animator.ResetTrigger("Shoot");
        _animator.ResetTrigger("Deactivate");
    }

    public void ResetChildComponents()
    {
        laserRenderer.enabled = false;
    }

    public void SetLineRendererDirection(Vector3 direction)
    {
        laserRenderer.SetPosition(0, attackPoint.position);
        laserRenderer.SetPosition(1, attackPoint.position + direction * attackRange);
    }
    #endregion 
}
