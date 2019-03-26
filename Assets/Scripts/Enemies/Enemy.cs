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
    private static List<Rigidbody2D> EnemyList;
    #endregion

    #region Enemy Variables
    [Header("Enemy Variables")]
    public float moveSpeed;
    public float repelRange;
    public float repelMultiplier;
    public float idleDuration;

    public int health;
    public int coinsDrop;

    public ENEMY_STATE _enemyState;

    private bool facingLeft;

    private Animator _animator;
    private Rigidbody2D _rBody;
    #endregion

    #region Attack Variables
    [Header("Attack Variables")]
    public float attackSpeed;
    public float attackRange;

    public int damage;

    public EnemyWeapon weapon;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rBody = GetComponent<Rigidbody2D>();
        weapon = GetComponent<EnemyWeapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_enemyState = ENEMY_STATE.IDLE;
        _enemyState = ENEMY_STATE.MOVE;

        if (EnemyList == null)
        {
            EnemyList = new List<Rigidbody2D>();
        }

        EnemyList.Add(_rBody);
    }

    private void OnDestroy()
	{
		EnemyList.Remove(_rBody);
	}

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        if (playerPos.x < _rBody.position.x && !FacingLeft()) SetFacingLeft(true);
        else if (playerPos.x > _rBody.position.x && FacingLeft()) SetFacingLeft(false);

        switch (_enemyState)
        {
            case ENEMY_STATE.IDLE:
                Idle();
                break;

            case ENEMY_STATE.MOVE:
                Move(playerPos);
                break;

            case ENEMY_STATE.ATTACK:
                Attack();
                break;

            case ENEMY_STATE.WANDER:
                break;

            case ENEMY_STATE.DEATH:
                break;
        }
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
    }

    // -------------------------------- Checkers --------------------------------
    
    public bool FacingLeft()
    {
        bool result = facingLeft;
        return result;
    }

    // -------------------------------- Functions --------------------------------
    
    public virtual IEnumerator Idle()
    {
        //_animator.SetTrigger("Idle"); //Play Idle Animation

        yield return new WaitForSeconds(5.0f);
    }

    public virtual void Attack()
    {

    }

    private void Reset()
    {
        SetFacingLeft(false);
    }

    private void Move(Vector2 playerPos)
    {
        float distance = Vector2.Distance(playerPos, transform.position);

        if (distance <= attackRange) return;

        Vector2 repelForce = Vector2.zero;

        for(int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == _rBody) continue;

            if(Vector2.Distance(EnemyList[i].position, _rBody.position) <= repelRange)
            {
                Vector2 repelDir = (_rBody.position - EnemyList[i].position).normalized;
                repelForce += repelDir;
            }
        }
        
        Vector2 moveDir = (playerPos - _rBody.position).normalized;
        Vector2 newPos = _rBody.position + moveDir * Time.fixedDeltaTime * moveSpeed;
        newPos += repelForce * Time.fixedDeltaTime * repelMultiplier;

        _rBody.MovePosition(newPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            SetEnemyState(ENEMY_STATE.IDLE);
        }
        else if(collision.tag == "Player")
        {
            SetEnemyState(ENEMY_STATE.ATTACK);
        }
    }
}
