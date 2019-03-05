using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserShooterState
{
    Idle,
    Active,
    Aiming,
    Charging,
    Shooting,
    Post_Shoot
}

public class LaserShooter : MonoBehaviour
{
    #region GameObject References
    public LineRenderer laserRenderer;
    public GameObject laserProjectile;
    protected Transform target;
    protected Animator _animator;
    #endregion

    #region Componenet's Variables
    [Header("Timer Variables")]
    public float shooterCooldown;
    protected float cooldownTimer;
    public float activeDuration;
    protected float activeTimer;
    public float chargingDuration;
    protected float chargingTimer;

    [Space(3)]
    [Header("Rotation Variables")]
    public float rotationSpeed;
    public float rotationOffset;

    [Space(3)]
    [Header("Attacking Variables")]
    public LayerMask attackLayer;
    public Transform attackPoint;
    public float attackRange;

    private RaycastHit2D hitInfo;
    private Vector3 shootDir;

    [Space(3)]
    [Header("Laser Color")]
    public Color originalColor;
    protected Color currentColor;
    private Vector3 colorHolder;

    public LaserShooterState state;
    #endregion

    #region Unity Functions
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Setting player as target
        _animator = gameObject.GetComponent<Animator>(); //Assigning animator

        state = LaserShooterState.Idle; //Set beginning state to Idle

        ResetTimers();
        ResetChildComponents();
    }
    
    void Update()
    {
        if (target == null)
            return;

        switch (state)
        {
            case LaserShooterState.Active:
                Active();
                break;

            case LaserShooterState.Aiming:
                Aiming();
                break;

            case LaserShooterState.Charging:
                Charging();
                break;

            case LaserShooterState.Shooting:
                Shooting();
                break;

            case LaserShooterState.Post_Shoot:
                PostShoot();
                break;

            default:
                Idle();
                break;
        }
    }
    #endregion

    #region Laser Shooter Behaviour
    void Idle()
    {
        ResetAllAnimationTriggers();

        cooldownTimer -= Time.deltaTime;
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position); //Calculate distance to target

        if (distanceToTarget <= attackRange && cooldownTimer <= 0.0f) //If target is within range & cooldown = 0, state = active
            state = LaserShooterState.Active;
    }

    void Active()
    {
        _animator.SetTrigger("Activate"); //Play Activate animation

        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0.0f)
            state = LaserShooterState.Aiming;
    }

    void Aiming()
    {
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position);

        if (distanceToTarget > attackRange)
        {
            if (laserRenderer.enabled == true)
                laserRenderer.enabled = false;

            return;
        }

        FaceTarget();

        //Enable laser line renderer
        laserRenderer.enabled = true;
        SetLineRendererDirection(transform.right);

        //Raycast
        hitInfo = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, attackLayer);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.tag == "Player")
            {
                SetShootDirection(hitInfo.point);
                state = LaserShooterState.Charging;
            }
        }
    }

    void Charging()
    {
        _animator.SetTrigger("Charge"); //Play Charging animation

        currentColor.g -= colorHolder.x / (chargingDuration / Time.deltaTime);
        currentColor.b -= colorHolder.y / (chargingDuration / Time.deltaTime);
        currentColor.a += (1 - colorHolder.z) / (chargingDuration / Time.deltaTime);

        Color tempColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        laserRenderer.startColor = tempColor;
        laserRenderer.endColor = tempColor;

        chargingTimer -= Time.deltaTime;
        if (chargingTimer <= 0.0f)
            state = LaserShooterState.Shooting;
    }

    void Shooting()
    {
        _animator.SetTrigger("Shoot"); //Play Shooting animation

        GameObject laser = Instantiate(laserProjectile, attackPoint.position, transform.rotation);
        laser.GetComponent<EnemyBullet>().SetShootDirection(GetShootDirection());

        ResetTimers();
        ResetChildComponents();

        state = LaserShooterState.Post_Shoot;
    }

    void PostShoot()
    {
        _animator.SetTrigger("Deactivate"); //Play Deactivate animation

        state = LaserShooterState.Idle;
    }
    #endregion

    #region Custom Functions
    void SetShootDirection(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;

        direction = direction.normalized;

        shootDir = direction;
    }

    Vector3 GetShootDirection()
    {
        return shootDir;
    }

    void FaceTarget()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    public void SetLineRendererDirection(Vector3 direction)
    {
        laserRenderer.SetPosition(0, attackPoint.position);
        laserRenderer.SetPosition(1, attackPoint.position + direction * attackRange);
    }

    public void ResetAllAnimationTriggers()
    {
        _animator.ResetTrigger("Activate");
        _animator.ResetTrigger("Charge");
        _animator.ResetTrigger("Shoot");
        _animator.ResetTrigger("Deactivate");
    }

    public void ResetTimers()
    {
        //Setting all timers to default value
        cooldownTimer = shooterCooldown;
        activeTimer = activeDuration;
        chargingTimer = chargingDuration;
    }

    public void ResetChildComponents()
    {
        laserRenderer.enabled = false;
        currentColor = originalColor;
        colorHolder = new Vector3(currentColor.g, currentColor.b, currentColor.a);
        laserRenderer.startColor = currentColor;
        laserRenderer.endColor = currentColor;
    }
    #endregion
}
