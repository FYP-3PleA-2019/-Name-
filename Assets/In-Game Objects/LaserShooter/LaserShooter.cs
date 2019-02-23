using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserShooterState
{
    Idle,
    Active,
    Aiming,
    Charging,
    Shooting
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
    public float activeRange;
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
    public Transform attackPoint;
    public float attackRange;

    private RaycastHit2D hitInfo;

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

            default:
                Idle();
                break;
        }
    }
    #endregion

    #region Laser Shooter Behaviour
    void Idle()
    {
        cooldownTimer -= Time.deltaTime;
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position); //Calculate distance to target

        if (distanceToTarget <= activeRange && cooldownTimer <= 0.0f) //If target is within range & cooldown = 0, state = active
            state = LaserShooterState.Active;
    }

    void Active()
    {
        // _animator.SetTrigger("Activate"); //Play Activate animation

        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0.0f)
            state = LaserShooterState.Aiming;
    }

    void Aiming()
    {
        FaceTarget();

        //Enable laser line renderer
        laserRenderer.enabled = true;
        SetLineRendererDirection(transform.up);

        //Raycast
        hitInfo = Physics2D.Raycast(attackPoint.position, transform.up, attackRange);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
                state = LaserShooterState.Charging;
        }
    }

    void Charging()
    {
        //_animator.SetTrigger("Charging"); //Play Charging animation

        currentColor.g -= colorHolder.x / (chargingDuration / Time.deltaTime);
        currentColor.b -= colorHolder.y / (chargingDuration / Time.deltaTime);
        currentColor.a += (1 - colorHolder.z) / (chargingDuration / Time.deltaTime);

        Color tempColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        laserRenderer.SetColors(tempColor, tempColor);

        chargingTimer -= Time.deltaTime;
        if (chargingTimer <= 0.0f)
            state = LaserShooterState.Shooting;
    }

    void Shooting()
    {
        //_animator.SetTrigger("Shooting"); //Play Shooting animation

        GameObject laser = Instantiate(laserProjectile, attackPoint.position, transform.rotation);

        ResetTimers();
        ResetChildComponents();

        //_animator.SetTrigger("Deactivate"); //Play Deactivate animation

        state = LaserShooterState.Idle;
    }

    #endregion

    #region Custom Functions
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
        laserRenderer.SetColors(currentColor, currentColor);
    }
    #endregion
}
