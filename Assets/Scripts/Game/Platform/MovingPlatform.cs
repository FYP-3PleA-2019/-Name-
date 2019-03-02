using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right
}

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    //Private Variables
    private GameObject player;
    private Vector2 moveTarget;

    //Public Variables
    public float moveRange;
    public float moveSpeed;
    public bool isGrounded;

    public MoveDirection _moveDirection;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moveTarget = ReturnMoveDirection();
        isGrounded = true;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveTarget, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(gameObject.transform);
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
            isGrounded = false;
        }
    }
    #endregion

    #region Custom Functions
    Vector2 ReturnMoveDirection()
    {
        Vector2 tempDir;

        if(_moveDirection == MoveDirection.Up)
            tempDir = new Vector2(transform.position.x, transform.position.y + moveRange);

        else if (_moveDirection == MoveDirection.Down)
            tempDir = new Vector2(transform.position.x, transform.position.y - moveRange);

        else if (_moveDirection == MoveDirection.Left)
            tempDir = new Vector2(transform.position.x - moveRange, transform.position.y);

        else
            tempDir = new Vector2(transform.position.x + moveRange, transform.position.y);

        return tempDir;
    }
    #endregion
}
