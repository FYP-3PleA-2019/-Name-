using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffPlatform : MonoBehaviour
{
    public GameObject platform;
    bool canLoad;

    private void Start()
    {
        canLoad = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (platform.GetComponent<MovingPlatform>().isGrounded == false && canLoad)
            {
                PlayerDetected();
            }
        }
    }

    void PlayerDetected()
    {
        canLoad = false;
        float damage = GameManager.Instance.player.controller.CurrHealth;
        GameManager.Instance.player.controller.GetDamage(damage, transform.position, 0f, 0f);
    }
}
