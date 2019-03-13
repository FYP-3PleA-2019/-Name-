using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornRainbow : Weapon
{
    public override IEnumerator Shoot(Transform shootPoint)
    {
        if(canShoot)
        {
            canShoot = false;


            while (canShoot && InputManager.Instance.IsShooting())
            {

            }
        }

    }
}
