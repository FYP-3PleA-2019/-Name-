using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : Projectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Enemy")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyBase>().ReceiveDamage(damage);

            Destroy(gameObject);
        }
    }
}
