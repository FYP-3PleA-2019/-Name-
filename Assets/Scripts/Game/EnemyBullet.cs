using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile
{
    public float damage;

    public override void Start()
    {
        //Used to override Projectile's Start function
    }

    public void SetShootDirection(Vector3 dir)
    {
        shootDir = dir;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerCoreController>().controller.GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
