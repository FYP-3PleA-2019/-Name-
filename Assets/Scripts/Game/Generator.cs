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
    [Space(3)]
    [Header("Manipulate-able GameObjects")]
    public GameObject movingPlatform;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Just for testing purposes
            InitiateGeneratorFunction();
    }

    public void InitiateGeneratorFunction()
    {
        //_animator.SetTrigger("isTriggered"); //Play triggered animation

        //Executes mechanic based on [Generator Type]
        if (_generatorType == GeneratorType.MovePlatform)
            Debug.Log("Moving Platform!");

        else if (_generatorType == GeneratorType.SpawnBridge)
            Debug.Log("Spawning Bridge!");
    }
}
