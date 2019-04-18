using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionUIController : MonoBehaviour
{
    private Canvas directionCanvas;
    private Animator _animator;

    private void Awake()
    {
        directionCanvas = GetComponent<Canvas>();
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationString)
    {
        _animator.SetTrigger(animationString);
    }

    public void EnableCanvas()
    {
        directionCanvas.enabled = true;
    }

    public void DisableCanvas()
    {
        directionCanvas.enabled = false;
    }
}
