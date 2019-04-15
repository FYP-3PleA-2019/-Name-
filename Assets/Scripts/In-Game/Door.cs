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

    public void OpenDoor()
    {
        Debug.Log("Door Opened!");
        _boxCollider.enabled = false;
        //_animator.SetTrigger("Open");
    }
}
