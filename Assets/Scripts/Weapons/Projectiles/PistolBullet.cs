using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : Projectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().ReceiveDamage(damage);

            Destroy(gameObject);
        }
    }
}
