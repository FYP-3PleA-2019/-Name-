﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentState
{
    Spawn,
    Idle,
    Move,
    Attack,
    Death,
}
public class EnemyBase : MonoBehaviour
{
    #region General Variables
    [Header("General Variables")]
    public CurrentState _currentState;
    private Animator _animator;
    private Transform target;
    private Transform currTarget;

    //Implement in KK's enemy
    public bool IsSpawned
    {
        get { return _isSpawned; }
        set
        {
            _isSpawned = value;
        }
    }
    private bool _isSpawned;

    private ComponentsRandomizer myParent;

    public float health;
    public float moveSpeed;
    public float wanderRange;

    public float minIdleTime;
    public float maxIdleTime;

    [HideInInspector]
    public List<GameObject> tempList;
    protected bool canCreate;

    protected float idleDuration;
    protected float idleTimer;
    #endregion

    #region Attack Variables
    [Header("Attack Variables")]
    public LayerMask attackLayer;
    public int attackDamage;
    public float attackRange;
    public float attackSpeed;

    protected float attackTimer;
    #endregion

    //Temporary
    public int myValue;
    public GameObject arrowIndicator;
    public Sprite indicatorSprite;
    #region Unity Functions
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        _currentState = CurrentState.Spawn; //Set initial state

        attackTimer = 0;

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        _animator = gameObject.GetComponent<Animator>();

        myParent = GetComponentsInParent<ComponentsRandomizer>()[0];

        attackTimer = attackSpeed;
        currTarget = target; //Setting current target to default target [Player]
        canCreate = true;

        //Temporary [Instantiate arrow indicator for this obj]
       // InstantiateArrowIndicator();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (target == null)
            return;

        switch (_currentState)
        {
            case CurrentState.Idle:
                Idle();
                break;

            case CurrentState.Move:
                Move();
                break;

            case CurrentState.Attack:
                Attack();
                break;

            case CurrentState.Death:
                Death();
                break;

            default:
                Spawn();
                break;
        }
    }
    #endregion

    #region Enemy Behaviour
    public virtual void Spawn()
    {
        //Spawn Particle Effects
        //Instantiate();

        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("SpawnAnimationName")) //Check to see if animator is still playing spawn animation
            _currentState = CurrentState.Idle; //Switch state to idle if spawning animation is finished playing
    }

    public virtual void Idle()
    {
        //_animator.SetTrigger("Idle"); //Play Idle Animation

        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration) //Short interval before moving
        {
            idleTimer = 0;
            _currentState = CurrentState.Move;
        }
    }

    public virtual void Move()
    {
        //_animator.SetTrigger("Move"); //Play Move Animation
        //Instantiate() //Spawn Particle effects at feet

        attackTimer += Time.deltaTime; //Increase attack timer

        if (attackTimer >= attackSpeed) //If ready to attack player, set [Player] as [Current Target]
        {
            currTarget = target;

            if (tempList.Count != 0)
            {
                Destroy(tempList[0]); //Destroy temporary [Game Object]
                tempList.RemoveAt(0);
                canCreate = true;
            }
        }

        else //Else, set [Wander Target] as [Current Target]
        {
            if (canCreate)
            {
                canCreate = false; 

                GameObject tempPos = new GameObject(); //Create new [GameObject] to hold temporary randomized position
                tempList.Add(tempPos); //Add created [GameObject] to a list 
                tempPos.transform.parent = gameObject.transform;

                Vector2 randWanderPos = RandomPositionWithinRadius(wanderRange, target.position); // Randomize a position in close proximity to the [Player]

                //check if randomized spot is tiled

                tempPos.transform.position = randWanderPos; //Set created [GameObject's] position to randomly generated position

                currTarget = tempPos.transform;
            }
        }

        float distanceFromTarget = Vector2.Distance(transform.position, currTarget.position); //Calculate distance between Enemy and [Current Target]
        transform.position = Vector2.MoveTowards(transform.position, currTarget.position, Time.deltaTime * moveSpeed); //Move towards [Current Target]

        if (distanceFromTarget <= attackRange) //If [Current Target] is within [Attack Range], 
        {
            if(currTarget.tag == "Player") //If [Current Target] is [Player]
            {
                _currentState = CurrentState.Attack; //Set state to [Attack]
            }

            else //Else, reposition self
            {
                Destroy(tempList[0]); //Destroy temporary [Game Object]
                tempList.RemoveAt(0);
                canCreate = true; 

                int randState = Random.Range(0, 2); // Gives Enemy a chance to Idle instead of constantly moving around

                if(randState == 0)
                {
                    idleDuration = Random.Range(minIdleTime, maxIdleTime);
                    _currentState = CurrentState.Idle;
                }
            }
        }
    }

    public virtual void Attack()
    {
        attackTimer = 0; //Reset Attack Timer

        //_animator.SetTrigger("Attack"); //Play Attack Animation

        //Temporary attack logic
        Collider2D[] gameObjectsToDamage = Physics2D.OverlapBoxAll(transform.position, new Vector2(attackRange * 2, attackRange * 2), 0, attackLayer); //Create an array of everything that is affected by this attack
        for (int i = 0; i < gameObjectsToDamage.Length; i++)
        {
            if (gameObjectsToDamage[i].gameObject.tag == "Player")
            {
                //gameObjectsToDamage[i].GetComponent<PlayerCoreController>().controller.GetDamage(attackDamage); //Damage all [Damage-able] gameobjects. NOTE! : Damage game objects by calling their Receive Damage function!
            }
        }
        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackAnimationName")) //Check to see if animator is still playing attack animation
        idleDuration = Random.Range(minIdleTime, maxIdleTime); //Random idle duration
        _currentState = CurrentState.Idle; //Switch state to idle if attack animation is finished playing
    }

    public virtual void Death()
    {
        //Play Death Animation 
        //_animator.SetTrigger("Death");

        //Spawn Particle Effects and spawn blood stains
        //Instantiate();
        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("DeathAnimationName")) //Check to see if animator is still playing death animation
        Destroy(gameObject);
    }
    #endregion

    #region Tools Functions
    public void ReceiveDamage(float damage)
    {
        if(health > 0)
            health -= damage;
        
        else if (health <= 0)
        {
            health = 0;

            if(_isSpawned)
            {
                myParent.ExistingEnemies--;
                myParent.CheckRoomClear();
            }

            _currentState = CurrentState.Death;
        }
    }

    public void FallOffPlatform()
    {
        _currentState = CurrentState.Death;
    }

    private Vector2 RandomPositionWithinRadius(float radius, Vector2 objectPos)
    {
        Vector2 rand = Random.insideUnitCircle * radius;
        Vector2 randSpawnPoint = new Vector2(objectPos.x + rand.x, objectPos.y + rand.y);

        return randSpawnPoint;
    }

    void InstantiateArrowIndicator()
    {
        GameObject indicator = Instantiate(arrowIndicator, transform.position, Quaternion.identity);
        indicator.transform.SetParent(this.transform);
        indicator.GetComponent<ArrowIndicator>().Target = this.transform;
        indicator.GetComponent<ArrowIndicator>().SpriteToDisplay = indicatorSprite;
    }
    #endregion

    #region Gizmos
    public virtual void OnDrawGizmosSelected() //Draw a cube that serves as an attack range indicator in the [Scene View]
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(attackRange * 2, attackRange * 2, 0));
    }
    #endregion
}
