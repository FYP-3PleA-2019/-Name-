using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devotion : Weapon
{
    public int shotsToActivate;
    public float maxFireRate;
    private float currFireRate;

    public override IEnumerator Shoot(Transform shootPoint)
    {
        if (GameManager.Instance.Coins - fireCost >= 0)
        {
            if (canShoot)
            {
                canShoot = false;
                currFireRate = fireRate;
            }
        }

        while (InputManager.Instance.IsShooting())
        {
            if (GameManager.Instance.Coins - fireCost >= 0)
            {
                GameManager.Instance.Coins -= fireCost;
                UIManager.Instance.coinUI.UpdateCoinUI();

                Vector3 shootPointRot = shootPoint.transform.rotation.eulerAngles;
                Vector3 bulletRot = new Vector3(shootPointRot.x, shootPointRot.y, shootPointRot.z + GetRandomSpread());
                Quaternion eulerRot = Quaternion.Euler(bulletRot);

                Projectile bullet = Instantiate(projectile, shootPoint.position, eulerRot).GetComponent<Projectile>();

                bullet.SetDamage(GetDamage());
                bullet.SetFireRange(GetFireRange());
                bullet.SetMoveSpeed(GetProjectileSpeed());
                yield return new WaitForSeconds(currFireRate);

                //Set new fire rate
                if (currFireRate > maxFireRate && !ReturnApproximation(currFireRate, maxFireRate, 0.005f))
                {
                    currFireRate -= (fireRate - maxFireRate) / shotsToActivate;
                }
            }
            else
            {
                canShoot = true;
                currFireRate = fireRate;
                yield return null;
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
