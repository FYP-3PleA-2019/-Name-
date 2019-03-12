using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    #region Variables
    public float rotationSpeed;

    [Range(0f, 300f)] public float distanceFromEdge;
    [Range(0f, 100f)] public float arrowDistanceFromEdge;

    public Transform arrow;

    private Transform player;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _arrowSpirteRenderer;
    private bool canSet;

    public Sprite SpriteToDisplay
    {
        get { return _spriteToDisplay; }
        set
        {
            _spriteToDisplay = value;
        }

    }
    private Sprite _spriteToDisplay;

    public Transform Target
    {
        get { return _target; }
        set
        {
            _target = value;
        }
    }
    private Transform _target;
    #endregion

    #region Unity Functions
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _arrowSpirteRenderer = arrow.GetComponent<SpriteRenderer>();
        SetSprite(_spriteToDisplay);
        DisableIndicators();
        canSet = true;
    }

    private void Update()
    {
        Vector3 targetPosition = Camera.main.WorldToScreenPoint(_target.position);

        if (IsOutOfBounds(targetPosition))
        {
            if (canSet)
            {
                canSet = false;
                EnableIndicators();
            }

            FaceTarget(arrow);
            MoveToTarget(targetPosition, this.transform, distanceFromEdge);
            MoveToTarget(targetPosition, arrow, arrowDistanceFromEdge);
        }

        else
        {
            if (!canSet)
            {
                canSet = true;
                DisableIndicators();
            }
        }
    }
    #endregion

    #region Custom Functions
    bool IsOutOfBounds(Vector3 targetPos)
    {
        if (targetPos.x <= 0 || targetPos.x >= Screen.width ||
            targetPos.y <= 0 || targetPos.y >= Screen.height)
            return true;

        else
            return false;
    }

    void MoveToTarget(Vector3 targetPos, Transform objToMove, float distanceFromBoundaries)
    {
        targetPos.x = Mathf.Clamp(targetPos.x, distanceFromBoundaries, Screen.width - distanceFromBoundaries);
        targetPos.y = Mathf.Clamp(targetPos.y, distanceFromBoundaries, Screen.height - distanceFromBoundaries);

        objToMove.position = Camera.main.ScreenToWorldPoint(targetPos);
    }

    void FaceTarget(Transform objToRotate)
    {
        Vector3 direction = _target.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        objToRotate.rotation = Quaternion.Slerp(objToRotate.rotation, q, Time.deltaTime * rotationSpeed);
    }

    void SetSprite(Sprite arrowIndicatorSprite)
    {
        _spriteRenderer.sprite = arrowIndicatorSprite;
    }

    void EnableIndicators()
    {
        _spriteRenderer.enabled = true;
        _arrowSpirteRenderer.enabled = true;
    }

    void DisableIndicators()
    {
        _spriteRenderer.enabled = false;
        _arrowSpirteRenderer.enabled = false;
    }
    #endregion
}
