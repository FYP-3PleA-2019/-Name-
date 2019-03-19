using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override IEnumerator Shoot()
    {
        while (canShoot && InputManager.Instance.IsShooting())
        {
            canShoot = false;

            Transform shootPoint = GameManager.Instance.player.weapon.GetShootPoint();

            Projectile bullet = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
            bullet.SetDamage(GetDamage());
            bullet.SetFireRange(GetFireRange());
            yield return new WaitForSeconds(GetFireRate());

            canShoot = true;
        }
    }
}
