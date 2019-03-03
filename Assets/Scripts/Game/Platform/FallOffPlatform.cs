using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffPlatform : MonoBehaviour
{
    public GameObject platform;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(platform.GetComponent<MovingPlatform>().isGrounded == false)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
