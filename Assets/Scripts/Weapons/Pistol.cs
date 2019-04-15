using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override IEnumerator Shoot(Transform shootPoint)
    {
        while (canShoot && InputManager.Instance.IsShooting())
        {
            SoundManager.instance.playSingle(SoundManager.instance.playerShoot);

            canShoot = false;
            
            Vector3 shootPointRot = shootPoint.transform.rotation.eulerAngles;
            Vector3 bulletRot = new Vector3(shootPointRot.x, shootPointRot.y, shootPointRot.z + GetRandomSpread());
            Quaternion eulerRot = Quaternion.Euler(bulletRot);

            Projectile bullet = Instantiate(projectile, shootPoint.position, eulerRot).GetComponent<Projectile>();
            bullet.SetDamage(GetDamage());
            bullet.SetFireRange(GetFireRange());
            bullet.SetMoveSpeed(GetProjectileSpeed());
            yield return new WaitForSeconds(GetFireRate());

            canShoot = true;
        }
    }
}
