using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Weapon
{
    public override IEnumerator Shoot(Transform shootPoint)
    {
        while (canShoot)
        {
            canShoot = false;

            Vector3 shootPointRot = shootPoint.transform.rotation.eulerAngles;
            Vector3 bulletRot = new Vector3(shootPointRot.x, shootPointRot.y, shootPointRot.z + GetRandomSpread());
            Quaternion eulerRot = Quaternion.Euler(bulletRot);

            Slash bullet = Instantiate(projectile, shootPoint.position, eulerRot).GetComponent<Slash>();
            //bullet.SetDamage(GetDamage());
            //bullet.SetFireRange(GetFireRange());
            yield return new WaitForSeconds(1.0f);
        }
        canShoot = true;
    }
}
