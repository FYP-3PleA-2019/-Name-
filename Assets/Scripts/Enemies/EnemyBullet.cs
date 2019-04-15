using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float moveSpeed;

    public float knockBackForce;
    public float knockBackDuration;

    protected Vector3 shootDir;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void Update()
    {
        transform.Translate(new Vector3(shootDir.x, shootDir.y) * Time.deltaTime * moveSpeed, Space.World);
    }

    public void SetShootDirection(Vector3 dir)
    {
        shootDir = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Enemy")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            Vector2 knockBackDir = new Vector2(other.transform.position.x, other.transform.position.y)
                                 - new Vector2(transform.position.x, transform.position.y);

            other.gameObject.GetComponent<PlayerCoreController>().controller.GetDamage(damage, knockBackDir, knockBackForce, knockBackDuration);
        }
    }
}
