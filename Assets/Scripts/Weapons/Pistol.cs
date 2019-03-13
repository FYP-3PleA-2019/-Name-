using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override IEnumerator Shoot(Transform shootPoint)
    {
        while (canShoot && InputManager.Instance.IsShooting())
        {
            canShoot = false;
            Projectile _projectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
            _projectile.SetDamage(GetDamage());
            _projectile.SetFireRange(GetFireRange());
            yield return new WaitForSeconds(GetFireRate());

            canShoot = true;
        }
    }
}
