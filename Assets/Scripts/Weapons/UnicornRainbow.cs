﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornRainbow : Weapon
{
    public Projectile laser;

    public override IEnumerator Shoot(Transform shootPoint)
    {
        if(canShoot)
        {
            SoundManager.instance.playSingle(SoundManager.instance.playerShoot);
            canShoot = false;
            
            laser = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
            laser.SetDamage(GetDamage());
            laser.SetFireRange(GetFireRange());
            laser.SetFireRate(GetFireRate());
            laser.SetMoveSpeed(GetProjectileSpeed());
        }

        while (InputManager.Instance.IsShooting())
        {
            yield return null;
        }
        
        Destroy(laser.gameObject);
        canShoot = true;
    }
}
