using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion 

    #region Generator Type Declaration
    [Space(3)]
    [Header("Generator Type")]
    public GeneratorType _generatorType;
    #endregion

    private void Start()
    {
        _animator = GetComponent<Animator>(); // Setting Generator's Animator Component

        if(_generatorType == GeneratorType.MovePlatform)
        {
            _movingPlatform = transform.parent.gameObject.GetComponent<MovingPlatform>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Just for testing purposes
            InitiateGeneratorFunction();
    }

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
}
