﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GeneratorType
{
    MovePlatform,
    SpawnBridge
}

public class Generator : MonoBehaviour
{
    #region Control-able GameObjects
    private MovingPlatform _movingPlatform;
    #endregion

    #region Generator Components
    [Space(3)]
    [Header("Personal Components")]
    private Animator _animator;
    public GameObject _canvas;
    #endregion 

    #region Generator Type Declaration
    [Space(3)]
    [Header("Generator Type")]
    public GeneratorType _generatorType;
    #endregion

    #region Unity Functions
    private void Start()
    {
        _animator = GetComponent<Animator>(); // Setting Generator's Animator Component

        DisableIndicator();

        if(_generatorType == GeneratorType.MovePlatform)
        {
            _movingPlatform = transform.parent.gameObject.GetComponent<MovingPlatform>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            InitiateGeneratorFunction();
        }
    }
    #endregion

    #region Custom Functions
    public void SetGeneratorType(int type)
    {
        if (type == 0)
            _generatorType = GeneratorType.MovePlatform;

        else
            _generatorType = GeneratorType.SpawnBridge;
    }

    public void InitiateGeneratorFunction()
    {
        //_animator.SetTrigger("isTriggered"); //Play triggered animation

        //Executes mechanic based on [Generator Type]
        if (_generatorType == GeneratorType.MovePlatform)
        {
            if (_movingPlatform.isGrounded && !_movingPlatform.isMoving)
                _movingPlatform.SetMoveTarget(_movingPlatform.moveRange);
        }

        else if (_generatorType == GeneratorType.SpawnBridge)
            Debug.Log("Spawning Bridge!");
    }

    public void ActivateIndicator()
    {
        _canvas.SetActive(true);
    }

    public void DisableIndicator()
    {
        _canvas.SetActive(false);
    }
    #endregion 
}
