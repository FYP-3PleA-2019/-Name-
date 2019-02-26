using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile
{
    public override void Start()
    {

    }

    public void SetShootDirection(Vector3 dir)
    {
        shootDir = dir;
    }
}
