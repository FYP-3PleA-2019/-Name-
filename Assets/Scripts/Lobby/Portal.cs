﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    public bool isActivated;
    public bool interactable;

    public GAME_SCENE gatewayTo;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActivated && collision.tag == "Player")
        {
            CustomSceneManager.Instance.LoadSceneWait(gatewayTo, 0.5f);
        }
    }
}
