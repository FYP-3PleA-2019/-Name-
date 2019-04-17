using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_STATE
{
    IDLE,
    MOVE,
    ATTACK,
    WANDER,
    DEATH,
}

public class Enemy : MonoBehaviour
{
    #region General Variables
    [Header("General Variables")]
    private ComponentsRandomizer myParent;

    public float maxIdleDuration;
    public float maxWanderDuration;

    private bool facingLeft;

    private Vector2 moveDir;
    private Vector2 retreatDir;
    private Vector2 repelDir;

    private Animator _animator;
    private EnemyWeaponController _enemyWeapon;
    private static List<Rigidbody2D> EnemyList;
    private Rigidbody2D _rBody;
    private Transform _weaponHolder;
    #endregion

    #region Enemy Variables
    [Header("Enemy Variables")]
    public float attackRange;
    public float moveSpeed;
    public float repelRange;
    public float repelMultiplier;

    public float health;
    public int coinsDrop;

    public int myValue;

    public bool IsSpawned
    {
        get { return _isSpawned; }
        set
        {
            _isSpawned = value;
        }
    }
    private bool _isSpawned;

    public ENEMY_STATE _enemyState;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyWeapon = GetComponent<EnemyWeaponController>();
        _rBody = GetComponent<Rigidbody2D>();
        _weaponHolder = GetComponentsInChildren<Transform>()[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        myParent = GetComponentsInParent<ComponentsRandomizer>()[0];

        if (EnemyList == null)
        {
            EnemyList = new List<Rigidbody2D>();
        }

        EnemyList.Add(_rBody);

        Reset();

        SetEnemyState(ENEMY_STATE.MOVE);
    }

    private void OnDestroy()
	{
		EnemyList.Remove(_rBody);
	}

    // Update is called once per frame
    void Update()
    {
        FacePlayer();

        StayAwayFromPlayer();

        StayAwayFromEnemies();

        switch (_enemyState)
        {
            case ENEMY_STATE.MOVE:
                Move();
                break;

            case ENEMY_STATE.ATTACK:
                Attack();
                break;

            default:
                break;
        }

        Vector2 dir = (moveDir + retreatDir + repelDir).normalized;
        Vector2 newPos = _rBody.position + dir * Time.fixedDeltaTime * moveSpeed;
        _rBody.MovePosition(newPos);
    }

    // -------------------------------- Setters --------------------------------

    //Set player facing
    void SetFacingLeft(bool facingLeft)
    {
        this.facingLeft = facingLeft;
        _animator.SetBool("facingLeft", facingLeft);
    }

    public void SetEnemyState(ENEMY_STATE enemyState)
    {
        _enemyState = enemyState;

        if (_enemyState == ENEMY_STATE.IDLE)
            StartCoroutine("Idle");

        else if (_enemyState == ENEMY_STATE.WANDER)
            StartCoroutine("Wander");

        else if (_enemyState == ENEMY_STATE.DEATH)
            StartCoroutine("Death");
    }

    // -------------------------------- Checkers --------------------------------
    
    public bool FacingLeft()
    {
        bool result = facingLeft;
        return result;
    }

    // -------------------------------- Functions --------------------------------
    private void Reset()
    {
        SetFacingLeft(false);

        moveDir = Vector2.zero;
        retreatDir = Vector2.zero;
        repelDir = Vector2.zero;

        _enemyWeapon.Reset();
    }

    private void FacePlayer()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        if (playerPos.x < _rBody.position.x && !FacingLeft())
        {
            SetFacingLeft(true);
            _enemyWeapon.SetFacingLeft(true);
        }
        else if (playerPos.x > _rBody.position.x && FacingLeft())
        {
            SetFacingLeft(false);
            _enemyWeapon.SetFacingLeft(false);
        }
    }

    private void StayAwayFromPlayer()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        float distance = Vector2.Distance(playerPos, transform.position);

        if (distance < attackRange - 0.1f) retreatDir = (_rBody.position - playerPos).normalized;
        else retreatDir = Vector2.zero;
    }

    private void StayAwayFromEnemies()
    {
        repelDir = Vector2.zero;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == _rBody) continue;

            if (Vector2.Distance(EnemyList[i].position, transform.position) < repelRange)
            {
                Vector2 repelForce  = (_rBody.position - EnemyList[i].position).normalized;
                repelDir += repelForce;
            }
        }
    }

    private IEnumerator Idle()
    {
        float randDuration = Random.Range(1.0f, maxIdleDuration);

        //_animator.SetTrigger("Idle"); //Play Idle Animation

        yield return new WaitForSeconds(randDuration);

        int randNum = Random.Range(0, 10);

        if (randNum >= 5) SetEnemyState(ENEMY_STATE.MOVE);
        else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
    }

    private void Move()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        float distance = Vector2.Distance(playerPos, _rBody.position);

        if (distance > attackRange + 0.1f) moveDir = (playerPos - _rBody.position).normalized;
        else
        {
            moveDir = Vector2.zero;
            SetEnemyState(ENEMY_STATE.ATTACK);
        }
    }

    private void Attack()
    {
        _enemyWeapon.Shoot();

        int randNum = Random.Range(0, 10);

        if (randNum >= 5) SetEnemyState(ENEMY_STATE.IDLE);
        else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
    }

    private IEnumerator Wander()
    {
        float randNormalizedX = Random.Range(-10, 10)/ 10f;
        float randNormalizedY = Random.Range(-10, 10)/ 10f;
        float randomDuration = Random.Range(1.0f, maxWanderDuration);
        
        moveDir = new Vector2(randNormalizedX, randNormalizedY);

        yield return new WaitForSeconds(randomDuration);

        int randNum = Random.Range(0, 10);

        if (randNum >= 8) SetEnemyState(ENEMY_STATE.WANDER);
        else if (randNum < 8) SetEnemyState(ENEMY_STATE.MOVE);
    }

    private IEnumerator Death()
    {
        //_animator.SetTrigger("Death"); //Play death Animation
        

        yield return new WaitForSeconds(0f);

        Destroy(gameObject);
    }

    public void ReceiveDamage(float damage)
    {
        if (health > 0)
            health -= damage;

        if (health <= 0)
        {
            health = 0;

            if (_isSpawned)
            {
                myParent.ExistingEnemies--;
                myParent.CheckRoomClear();
            }

            SetEnemyState(ENEMY_STATE.DEATH);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rBody.velocity = Vector3.zero;
        
        int randNum = Random.Range(0, 10);

        if(_enemyState == ENEMY_STATE.MOVE)
        {
            if (randNum >= 5) SetEnemyState(ENEMY_STATE.IDLE);
            else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
        }
        else if(_enemyState == ENEMY_STATE.WANDER)
        {
            if (randNum >= 5) SetEnemyState(ENEMY_STATE.MOVE);
            else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _rBody.velocity = Vector3.zero;

        int randNum = Random.Range(0, 10);

        if (_enemyState == ENEMY_STATE.MOVE)
        {
            if (randNum >= 5) SetEnemyState(ENEMY_STATE.IDLE);
            else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
        }
        else if (_enemyState == ENEMY_STATE.WANDER)
        {
            if (randNum >= 5) SetEnemyState(ENEMY_STATE.MOVE);
            else if (randNum < 5) SetEnemyState(ENEMY_STATE.WANDER);
        }
    }
}
