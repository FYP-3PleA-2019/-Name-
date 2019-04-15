using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Projectile
{
    private bool canKnockBack;

    private void Start()
    {
        canKnockBack = true;

        StartCoroutine(WaitToDestroy());
    }

    public override void Update()
    {

    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.15f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canKnockBack)
        {
            canKnockBack = false;
            
            Vector2 knockBackDir = new Vector2(other.transform.position.x, other.transform.position.y)
                                 - new Vector2(transform.position.x, transform.position.y);
            
            other.gameObject.GetComponent<PlayerCoreController>().controller.GetDamage(damage, knockBackDir, 10.0f, 0.2f);
        }
    }
}
