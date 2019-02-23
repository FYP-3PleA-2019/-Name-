﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    public float moveSpeed;

    protected Vector3 shootDir;
    #endregion

    // Start is called before the first frame update
    public virtual void Start()
    {
        shootDir = InputManager.Instance.GetShootDir();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.Translate(new Vector3(shootDir.x, shootDir.y) * Time.deltaTime * moveSpeed, Space.World);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
