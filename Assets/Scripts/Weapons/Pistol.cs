using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    //private void Start()
    //{
    //    _animator = GameObject.FindWithTag("Player").GetComponentsInChildren<Animator>()[1];
    //}

    public override IEnumerator Shoot(Transform shootPoint)
    {
        while (canShoot && InputManager.Instance.IsShooting())
        {
            if (GameManager.Instance.Coins - fireCost >= 0)
            {
                SoundManager.instance.playSingle(SoundManager.instance.playerShoot);

                canShoot = false;

                GameManager.Instance.Coins -= fireCost;
                UIManager.Instance.coinUI.UpdateCoinUI();

                Vector3 shootPointRot = shootPoint.transform.rotation.eulerAngles;
                Vector3 bulletRot = new Vector3(shootPointRot.x, shootPointRot.y, shootPointRot.z + GetRandomSpread());
                Quaternion eulerRot = Quaternion.Euler(bulletRot);

                Projectile bullet = Instantiate(projectile, shootPoint.position, eulerRot).GetComponent<Projectile>();
                bullet.SetDamage(GetDamage());
                bullet.SetFireRange(GetFireRange());
                bullet.SetMoveSpeed(GetProjectileSpeed());

                // _animator.SetTrigger("Shoot");

                yield return new WaitForSeconds(GetFireRate());

                canShoot = true;
            }
            else
            {
                canShoot = true;
                yield return null;
            }
        }
    }
}
