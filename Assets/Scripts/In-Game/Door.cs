using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.parent = gameObject.transform.root;
    }

    public void OpenDoor()
    {
        Debug.Log("Door Opened!");
        _boxCollider.enabled = false;
        UIManager.Instance.directionUI.EnableCanvas();

        //Direction UI
        ComponentsRandomizer tempParent = transform.parent.GetComponent<ComponentsRandomizer>();

        if(tempParent._spawnPoint == SpawnPoint.Top)
            UIManager.Instance.directionUI.PlayAnimation("Up");

        else if (tempParent._spawnPoint == SpawnPoint.Left)
            UIManager.Instance.directionUI.PlayAnimation("Left");

        else if (tempParent._spawnPoint == SpawnPoint.Right)
            UIManager.Instance.directionUI.PlayAnimation("Right");

        //_animator.SetTrigger("Open");
    }
}
