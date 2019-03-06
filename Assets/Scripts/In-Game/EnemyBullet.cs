﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float moveSpeed;

    protected Vector3 shootDir;

    public virtual void Update()
    {
        transform.Translate(new Vector3(shootDir.x, shootDir.y) * Time.deltaTime * moveSpeed, Space.World);
    }

    public void SetShootDirection(Vector3 dir)
    {
        shootDir = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerCoreController>().controller.GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
