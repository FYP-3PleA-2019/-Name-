using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devotion : Weapon
{
    public int shotsToActivate;
    public float maxFireRate;
    private float currFireRate;

    public override IEnumerator Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            currFireRate = fireRate;
        }

        while (InputManager.Instance.IsShooting())
        {
            Transform shootPoint = GameManager.Instance.player.weapon.GetShootPoint();

            Projectile bullet = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
            bullet.SetDamage(GetDamage());
            bullet.SetFireRange(GetFireRange());
            yield return new WaitForSeconds(currFireRate);

            //Set new fire rate
            if (currFireRate > maxFireRate && !ReturnApproximation(currFireRate, maxFireRate, 0.005f))
            {
                currFireRate -= (fireRate - maxFireRate) / shotsToActivate;
                print(currFireRate);
            }
        }

        canShoot = true;
    }

    #region Tools
    bool ReturnApproximation(float a, float b, float value) //Used for checking if 2 seperate values are similar.
    {
        return (Mathf.Abs(a - b) < value);
    }
    #endregion 
}
