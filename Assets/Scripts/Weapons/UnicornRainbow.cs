using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornRainbow : Weapon
{
    public Projectile laser;
    float timer;

    public override IEnumerator Shoot(Transform shootPoint)
    {
        if (GameManager.Instance.Coins - fireCost >= 0)
        {
            if (canShoot)
            {
                SoundManager.instance.playSingle(SoundManager.instance.playerShoot);
                canShoot = false;
                timer = fireRate;

                laser = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Projectile>();
                laser.SetDamage(GetDamage());
                laser.SetFireRange(GetFireRange());
                laser.SetFireRate(GetFireRate());
                laser.SetMoveSpeed(GetProjectileSpeed());
            }
        }

        while (InputManager.Instance.IsShooting())
        {
            if (GameManager.Instance.Coins - fireCost >= 0)
            {
                timer += Time.deltaTime;
                if (timer >= fireRate)
                {
                    timer = 0f;
                    GameManager.Instance.Coins -= fireCost;
                    UIManager.Instance.coinUI.UpdateCoinUI();
                }
            }
            else
            {
                timer = 0f;
                if(laser != null) Destroy(laser.gameObject);
                canShoot = true;
            }

            yield return null;
        }

        if (laser != null) Destroy(laser.gameObject);
        canShoot = true;
    }
}
