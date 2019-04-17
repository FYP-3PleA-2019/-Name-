using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENTITY_STATE
{
    IDLE,
    MOVE,
    STOP,
}

public class TheEntity : MonoBehaviour, IObserver
{
    #region General
    [Header("General")]
    public float moveSpeed;

    public float constantDistWithPlayer;
    public float minDistToShakeCamera;
    
    public Transform[] childList;

    public Camera cam;
    private Vector3 halfScreenHeight;

    public ENTITY_STATE currEntityState;
    private ENTITY_STATE prevEntityState;

    //Knockback
    public float knockBackForce;
    public float knockBackDuration;

    public float bufferSize;
    #endregion

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        childList = GetComponentsInChildren<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.player.controller.RegisterObserver(this);

        Vector3 temp = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        constantDistWithPlayer = temp.y + 5.0f;
        //constantDistWithPlayer = GameManager.Instance.player.transform.position.y - transform.position.y;
        halfScreenHeight = cam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2.0f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        switch (currEntityState)
        {
            case ENTITY_STATE.IDLE:
                SetChildSprRenderer(false);
                FollowPlayer();
                break;

            case ENTITY_STATE.MOVE:
                SetChildSprRenderer(true);
                Move();
                break;

            case ENTITY_STATE.STOP:
                float entityYPos = transform.position.y;
                float playerYPos = GameManager.Instance.player.transform.position.y;
                if(entityYPos < playerYPos + bufferSize)
                    Move();
                break;

            default:
                SetEntityState(ENTITY_STATE.IDLE);
                break;
        }
    }

    public void SetEntityState(ENTITY_STATE newEntityState)
    {
        prevEntityState = currEntityState;
        currEntityState = newEntityState;
    }

    public void OnNotify(NOTIFY_TYPE type)
    {
        if (type == NOTIFY_TYPE.ENTITY_IDLE)
        {
            if (!CheckChildRenderer())
            {
                SetEntityState(ENTITY_STATE.IDLE);
            }
        }
        else if (type == NOTIFY_TYPE.ENTITY_MOVE)
        {
            SetEntityState(ENTITY_STATE.MOVE);
        }

        else if(type == NOTIFY_TYPE.GAME_OVER)
        {
            SetEntityState(ENTITY_STATE.STOP);
        }
    }

    private bool CheckChildRenderer()
    {
        for(int i = 1; i < childList.Length; i++)
        {
            /*if (childList[i].GetComponent<Renderer>().isVisible)
                return true;*/

            float playerPosY = GameManager.Instance.player.transform.position.y;
            if (childList[i] != transform)
            {
                if (playerPosY - childList[i].position.y < constantDistWithPlayer - 3.0f)
                    return true;
            }
        }
        return false;
    }

    private void SetChildSprRenderer(bool active)
    {
        for(int i = 1; i < childList.Length; i++)
        {
            if(childList[i] != transform)
                childList[i].GetComponent<SpriteRenderer>().enabled = active;
        }
    }

    private void FollowPlayer()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        transform.position = new Vector3(playerPos.x, playerPos.y - constantDistWithPlayer, 0f);
    }
    
    private void Move()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        if (transform.position.y < playerPos.y)
            transform.position = new Vector3(playerPos.x, transform.position.y + (moveSpeed * Time.deltaTime), 0f);

        for (int i = 1; i < childList.Length; i++)
        {
            Vector3 dir = GameManager.Instance.player.transform.position - childList[i].position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            childList[i].rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        CalculateCameraShake();
    }

    private void CalculateCameraShake()
    {
        float distance = Vector2.Distance(GameManager.Instance.player.transform.position, transform.position);

        if(distance <= minDistToShakeCamera)
        {
            float index = constantDistWithPlayer - distance;

            Camera.main.GetComponent<MainCamera>().CameraShake(index, distance, minDistToShakeCamera);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float damage = GameManager.Instance.player.controller.CurrHealth;

            Vector2 knockBackDir = new Vector2(collision.transform.position.x, collision.transform.position.y)
                                 - new Vector2(transform.position.x, transform.position.y);

            GameManager.Instance.player.controller.GetDamage(damage, knockBackDir, knockBackForce, knockBackDuration);
        }
        else if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().ReceiveDamage(1000f);
        }
    }
}
