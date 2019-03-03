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

public enum Interaction
{
    Non_Interactable,
    Interactable
}

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    //Private Variables
    private Vector3 moveTarget;

    //Public Variables
    public GameObject generator;
    public Transform generatorSpawnPoint;
    public float moveRange;
    public float moveSpeed;

    [HideInInspector]
    public bool isGrounded, isMoving;

    public MoveDirection _moveDirection;
    public Interaction _interaction;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        ResetMoveTarget();
        isGrounded = true;
        isMoving = false;

        if (_interaction == Interaction.Interactable)
        {
            GameObject GO = Instantiate(generator, generatorSpawnPoint.position, Quaternion.identity);
            GO.transform.parent = gameObject.transform;
            GO.GetComponent<Generator>().SetGeneratorType(0);
        }

        else
        {
            SetMoveTarget(moveRange);
        }
    }

    private void Update()
    {
        if (transform.position != moveTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, Time.deltaTime * moveSpeed);
            isMoving = true;
        }

        else
        {
            isMoving = false;
        }
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
    Vector3 ReturnMoveDirection(float moveDistance)
    {
        Vector3 tempDir;

        if(_moveDirection == MoveDirection.Up)
            tempDir = new Vector3(moveTarget.x, moveTarget.y + moveDistance, 0.0f);

        else if (_moveDirection == MoveDirection.Down)
            tempDir = new Vector3(moveTarget.x, moveTarget.y - moveDistance, 0.0f);

        else if (_moveDirection == MoveDirection.Left)
            tempDir = new Vector3(moveTarget.x - moveDistance, moveTarget.y, 0.0f);

        else
            tempDir = new Vector3(moveTarget.x + moveDistance, moveTarget.y, 0.0f);

        return tempDir;
    }

    bool ReturnCanMove() //Check if platform will go out of room bounds
    {
        float parentRoomSize = transform.root.gameObject.GetComponent<BoxCollider2D>().size.x; //Retrieve room's size
        float myRoomBounds = ReturnMoveDirection(moveRange).x + 2;

        if (myRoomBounds <= parentRoomSize)
            return true;

        else
            return false;
    }

    void ResetMoveTarget()
    {
        moveTarget = transform.position;
    }

    public void SetMoveTarget(float moveDistance)
    {
        if (!ReturnCanMove())
            return;

        Vector3 tempVector = ReturnMoveDirection(moveDistance);
        moveTarget = tempVector;
    }
    #endregion
}
