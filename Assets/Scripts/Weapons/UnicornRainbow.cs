using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornRainbow : Weapon
{
    public Projectile laser;

    public override IEnumerator Shoot()
    {
        if(canShoot)
        {
            canShoot = false;

            Transform shootPoint = GameManager.Instance.player.weapon.GetShootPoint();

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
